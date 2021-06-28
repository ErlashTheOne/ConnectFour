"use strict";
let lobbyId = getParameterByName("lobby");

let where = getParameterByName("where");

var turn = "player-red";

var movementCont = 0;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();

if (lobbyId === "random") {
    connection.on("GetRandomLobbyId", function (randomLobbyId) {
        lobbyId = randomLobbyId;
    });

    document.querySelector(".lobbyId h2").style.display = "none";
    document.querySelector(".lobbyId .customTooltip").style.display = "none";
}
else {
    document.querySelector("#roomId").value = lobbyId;
}

if (where === "joinRandom") {
    connection.start().then(res => {
         connection.invoke("JoinQuickPlay")
              .catch(err => {
                    console.log(err);
              }
         );
    }).catch(err => {
        console.log(err);
    });
}
else {
    connection.start().then(res => {
        if (where === "create") {
            connection.invoke("CheckLobbies", lobbyId).catch(err => {
                    console.log(err);
            });
        }
        else {
            connection.invoke("CheckPlayers", lobbyId).catch(err => {
                    console.log(err);
             });
        }
    }).catch(err => {
        console.log(err);
    });
}

let xButton = document.querySelector("#xButton");
xButton.addEventListener("click", function () {
    connection.invoke("RemoveQuickPlayLobby", lobbyId)
    connection.invoke("RemoveLobby", lobbyId)
    window.location.href = `/Index`;
});
window.onbeforeunload = function () {
    connection.invoke("RemoveQuickPlayLobby", lobbyId)
    connection.invoke("RemoveLobby", lobbyId)
    window.location.href = `/Index`;
};

connection.on("RedirectWithMessage", function (message) {
    modal.style.display = "block";
    document.querySelector("#result").innerHTML = message;
});


let playerToken;
let enemyToken;

connection.on("AssignPlayer", function (player) {
    if (player === 1) {
        playerToken = "player-red";
        enemyToken = "player-black";
    } else if (player === 2) {
        playerToken = "player-black";
        enemyToken = "player-red";
    }

    document.querySelector(".gameBoard").classList.add(playerToken + "-token");
    document.querySelector(".gameBoard-container").classList.add(playerToken + "-token");

    
    if (turn == playerToken) {
        document.querySelector(".gameBoard").classList.add("current-turn");
        document.querySelector(".gameBoard-container").classList.add("current-turn");
    }

    

});

connection.on("OpenLobby", function () {
    document.getElementById("loader").style.display = "none";
    document.querySelector(".lobbyId").style.display = "none";
    document.querySelector(".gameInformation").style.display = "block";
    document.querySelector(".gameBoard").style.display = "flex";
    if (turn == playerToken) {
        document.querySelector("#YourTurn").style.display = "block";
    } else {
        document.querySelector("#EnemyTurn").style.display = "block";
    }
    dropToken();
});

var stopCountDown;
function CountDown() {
    
    var timeLeft = 40;
     stopCountDown = setInterval(function () {
        document.querySelector("#userCountDown").innerHTML = timeLeft;
        timeLeft = timeLeft-1;


         if (timeLeft < 0) {
             clearInterval(stopCountDown);
             if (turn == "player-red") {
                 gameOver("player-black");
             } else {
                 gameOver("player-red");
             }
         }
    }, 1000);
}

function dropToken() {

    CountDown();
    let column = document.querySelectorAll('.gameBoard .column');

    for (let i = 0; i < column.length; i++) {
        column[i].addEventListener("click", function () {
            if (turn == playerToken) {
                let columnPosition = i;
                let firstDiv = this.firstElementChild;

                if (!firstDiv.classList.contains("taken")) {
                    connection.invoke("SendMove", columnPosition, lobbyId).catch(function (err) {
                        return console.error(err.toString());
                    });
                    movementCont++;
                }
            }
        });
    }
}

connection.on("ReceiveMove", function (columnPosition) {
    clearInterval(stopCountDown)
    let selectedColumn = document.querySelectorAll('.gameBoard .column')[columnPosition];
    let squares = selectedColumn.children;
    for (let j = 0; j < squares.length; j++) {
        if (!squares[j].classList.contains("taken")) {
            if (j !== 0) {
                squares[j - 1].classList.remove("taken");
                if (turn === "player-red") {
                    squares[j - 1].classList.remove("player-red");
                } else {
                    squares[j - 1].classList.remove("player-black");
                }
            }
            squares[j].classList.add("taken");
            if (turn === "player-red") {
                squares[j].classList.add("player-red");
            } else {
                squares[j].classList.add("player-black");
            }
        }
    }


    changeTurn();
    checkBoard();
});


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function checkBoard() {
    const modal = document.getElementById("myModal");
    const square = document.querySelectorAll(".square");
    const winningArrays = [
        [0, 1, 2, 3], [1, 2, 3, 4], [2, 3, 4, 5],
        [6, 7, 8, 9], [7, 8, 9, 10], [8, 9, 10, 11],
        [12, 13, 14, 15], [13, 14, 15, 16], [14, 15, 16, 17],
        [18, 19, 20, 21], [19, 20, 21, 22], [20, 21, 22, 23],
        [24, 25, 26, 27], [25, 26, 27, 28], [26, 27, 28, 29],
        [30, 31, 32, 33], [31, 32, 33, 34], [32, 33, 34, 35],
        [36, 37, 38, 39], [37, 38, 39, 40], [38, 39, 40, 41],

        [0, 6, 12, 18], [6, 12, 18, 24], [12, 18, 24, 30], [18, 24, 30, 36],
        [1, 7, 13, 19], [7, 13, 19, 25], [13, 19, 25, 31], [19, 25, 31, 37],
        [2, 8, 14, 20], [8, 14, 20, 26], [14, 20, 26, 32], [20, 26, 32, 38],
        [3, 9, 15, 21], [9, 15, 21, 27], [15, 21, 27, 33], [21, 27, 33, 39],
        [4, 10, 16, 22], [10, 16, 22, 28], [16, 22, 28, 34], [22, 28, 34, 40],
        [5, 11, 17, 23], [11, 17, 23, 29], [17, 23, 29, 35], [23, 29, 35, 41],

        [2, 9, 16, 23],
        [1, 8, 15, 22], [8, 15, 22, 29],
        [0, 7, 14, 21], [7, 14, 21, 28], [14, 21, 28, 35],
        [6, 13, 20, 27], [13, 20, 27, 34], [20, 27, 34, 41],
        [12, 19, 26, 33], [19, 26, 33, 40],
        [18, 25, 32, 39],

        [3, 8, 13, 18],
        [4, 9, 14, 19], [9, 14, 19, 24],
        [5, 10, 15, 20], [10, 15, 20, 25], [15, 20, 25, 30],
        [11, 16, 21, 26], [16, 21, 26, 31], [21, 26, 31, 36],
        [17, 22, 27, 32], [22, 27, 32, 37],
        [23, 28, 33, 38]
    ];

    for (let y = 0; y < winningArrays.length; y++) {
        const square1 = square[winningArrays[y][0]];
        const square2 = square[winningArrays[y][1]];
        const square3 = square[winningArrays[y][2]];
        const square4 = square[winningArrays[y][3]];

        if (square1.classList.contains('player-red') &&
            square2.classList.contains('player-red') &&
            square3.classList.contains('player-red') &&
            square4.classList.contains('player-red')) {

            gameOver('player-red');
        }
        else
        if (square1.classList.contains('player-black') &&
            square2.classList.contains('player-black') &&
            square3.classList.contains('player-black') &&
            square4.classList.contains('player-black')) {

            gameOver('player-black');
        }
    }

    const squareLastLine1 = square[0];
    const squareLastLine2 = square[6];
    const squareLastLine3 = square[12];
    const squareLastLine4 = square[18];
    const squareLastLine5 = square[24];
    const squareLastLine6 = square[30];
    const squareLastLine7 = square[36];

    if (squareLastLine1.classList.contains('taken') &&
        squareLastLine2.classList.contains('taken') &&
        squareLastLine3.classList.contains('taken') &&
        squareLastLine4.classList.contains('taken') &&
        squareLastLine5.classList.contains('taken') &&
        squareLastLine6.classList.contains('taken') &&
        squareLastLine7.classList.contains('taken')) {
        gameOver('tie');
    }
}

function changeTurn() {
    document.querySelector("#YourTurn").style.display = "none";
    document.querySelector("#EnemyTurn").style.display = "none";
    let currentTurnArray = document.querySelectorAll(".current-turn");
    if (currentTurnArray) {
        for (let l = 0; l < currentTurnArray.length; l++) {
            currentTurnArray[l].classList.remove("current-turn");
        }
    }
    if (turn === "player-red") {
        turn = "player-black";
    } else {
        turn = "player-red";
    }
    if (turn == playerToken) {
        document.querySelector(".gameBoard").classList.add("current-turn");
        document.querySelector(".gameBoard-container").classList.add("current-turn");
        document.querySelector("#YourTurn").style.display = "block";
    } else {
        document.querySelector("#EnemyTurn").style.display = "block";
    }
    CountDown();
}

function gameOver(winner) {

    document.querySelector("#userCountDown").style.display = "none";

    sendGameData(winner);

    connection.invoke("RemoveLobby", lobbyId).catch(function (err) {
        return console.error(err.toString());
    });

    modal.style.display = "block";
    let gameOverString;
    if (winner === playerToken) {
        gameOverString = "You Win!";
    } else if (winner === "tie") {
        gameOverString = "It is a Tie.";
    } else {
        gameOverString = "You Lose!";
    }

    document.querySelector("#result").innerHTML = gameOverString;
}


function sendGameData(winner){
    const gameOverConnection = new signalR.HubConnectionBuilder()
        .withUrl("/playerHub")
        .build();

    let win = 0;
    let lose = 0;
    let tie = 0;

    if (winner === playerToken) {
        win++;
    }
    else if (winner === "tie") {
        tie++;
        movementCont = 0;
    }
    else {
        lose++;
        movementCont = 0;
    }

    gameOverConnection.start().then(res => {
        gameOverConnection.invoke("ReceiveGameData", win, lose, tie, movementCont).catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(err => {
        console.log(err);
    });
    
} 