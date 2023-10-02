'use strict';

export const apiCall = async (route, method, body) => {
    const authApi = 'http://localhost:5214'
    return await fetch(`${authApi}${route}`, {
        method,
        credentials: 'include',
        body,
        headers: {
            'content-type': 'application/json'
        }
    })
}

export const redirect = (route) => {
    window.location.replace(`${route}.html`);
}
