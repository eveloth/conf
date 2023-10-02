'use strict';

import { apiCall, redirect } from "./lib.js";

const loggedIn = await apiCall('/info', 'GET').then(x => x.ok);

if (loggedIn) redirect('/user');

const loginForm = document.getElementById('login-form');
const loginButton = document.getElementById('login-button');

console.log('init listener');
loginButton.addEventListener('click', async e => {
    e.preventDefault();

    const credentials = {
        username: loginForm.username.value,
        password: loginForm.password.value
    };

    const response = await apiCall('/login', 'POST', JSON.stringify(credentials));

    if (response.status === 401) {
        alert('nope.');
        return;
    }

    redirect('/user');
});
