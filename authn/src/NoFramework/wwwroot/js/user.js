'use strict';

import { apiCall, redirect } from "./lib.js";

const infoBlock = document.querySelector('.userinfo');
const idBox = document.querySelector('.id_box');
const username = document.createElement('p');
const password = document.createElement('p');
const logoutButton = document.getElementById('logout-button');

const response = await apiCall('/info', 'GET');

if (!response.ok) redirect('/index')

const userInfo = await response.json();

username.textContent = 'Welcome, ' + userInfo.username + '!';
password.textContent = `We know your password btw: ${userInfo.password}`;

idBox.innerHTML = `<h1>${userInfo.id}</h1>`;
infoBlock.appendChild(username);
infoBlock.appendChild(password);

logoutButton.addEventListener('click', async e => {
    e.preventDefault();

    await apiCall('/logout', 'POST');

    redirect('/index')
});
