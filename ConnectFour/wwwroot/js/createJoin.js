let createButton = document.querySelector("#createButton");
let lobbyId = getRandomInt(100000, 999999); //Random between 999999 and 100000
createButton.addEventListener("click", function () {
    window.location.href = `/Game?lobby=${lobbyId}&where=create`;
});

let joinLobbyId = document.querySelector("#joinForm input[type='text']");
let searchingLobbyId = document.querySelector("#joinForm input[type='submit']");

searchingLobbyId.addEventListener("click", function (event) {
    event.preventDefault();
    window.location.href = `/Game?lobby=${joinLobbyId.value}&where=join`;
});

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}


