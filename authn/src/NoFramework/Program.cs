using Microsoft.AspNetCore.Mvc;
using NoFramework;

var db = new Database();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles(
    new StaticFileOptions
    {
        OnPrepareResponse = context =>
            context.Context.Response.Headers.Add("cache-control", "no cache")
    }
);

app.MapPost(
    "/register",
    ([FromBody] UserRequest request) =>
    {
        var user = new User(request.Username, request.Password);
        var createdUser = db.Add(user);
        return Results.Ok(createdUser.Id);
    }
);

app.MapPost(
    "/login",
    async (HttpContext ctx, [FromBody] UserRequest request) =>
    {
        #region Database check

        var user = db[request.Username];

        if (user is null)
        {
            return Results.Unauthorized();
        }

        if (user.Password != request.Password)
        {
            return Results.Unauthorized();
        }

        #endregion

        ctx.Response.Headers["set-cookie"] = $"authn={user.Id}";

        return Results.Ok($"Welcome, {request.Username}!");
    }
);

app.MapGet(
    "/info",
    async (HttpContext ctx) =>
    {
        // получаем заголовок "Cookie" - то есть, строку (в данном случае StringValues, будь оно неладно, но пофиг)
        var cookieHeader = ctx.Request.Headers.Cookie;

        // достаём из этой строки нужную нам - а именно ту, которая начинается с "auth", названия нашей куки
        var cookieString = cookieHeader.FirstOrDefault(x => x?.StartsWith("authn") ?? false);

        // разбиваем ее по '=' и достаём значение по индексу 1
        var cookieValue = cookieString?.Split('=').ElementAtOrDefault(1);

        // если ничего не нашлось, куки нет, а значит пользователь не аутентифицирован
        if (cookieValue is null)
        {
            return Results.Unauthorized();
        }

        // приводим строковое значение id из куки к целому числу
        var id = int.Parse(cookieValue);

        // забираем пользователя из базы данных
        var user = db[id];

        // возвращаем с кодом ответа 200 и пользователем в теле ответа, если пользователь найден
        return user is null ? Results.Unauthorized() : Results.Ok(user);
    }
);

app.MapPost(
    "/logout",
    async (HttpContext ctx) =>
    {
        // удаляем куку - то есть, просто прописываем ей пустое значение после '='
        ctx.Response.Headers["set-cookie"] = "authn=; expires=Thu, 01 Jan 1970 00:00:00 GMT";

        return Results.Ok();
    }
);

await app.RunAsync();
