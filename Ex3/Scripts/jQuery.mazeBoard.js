
(function ($) {
    var canvas;
    var context;
    var rows;
    var cols;
    var cellWidth;
    var cellHeight;
    var mazeDat;
    var strRow;
    var strCol;
    var xitRow;
    var xitCol;
    var playerImg = new Image();
    playerImg.src = "../Images/Mario.png";
    var exitImg = new Image();
    exitImg.src = "../Images/exit.jpg";
    var playerRowLoc;
    var playerColLoc;
    var Rival;
    var Multi;
    var messagesHub = $.connection.multiplayerHub;
    $.connection.hub.start().done(function () {});

    $.fn.mazeBoard = function (mazeData, startRow, startCol, exitRow, exitCol, enabled,multi=false,rival="") {
        canvas = $(this)[0];
        context = canvas.getContext("2d");
        context.clearRect(0, 0, canvas.width, canvas.height);
        rows = mazeData.length;
        cols = mazeData[0].length;
        cellWidth = mazeCanvas.width / cols;
        cellHeight = mazeCanvas.height / rows;
        mazeDat = mazeData;
        strRow = startRow;
        strCol = startCol;
        xitRow = exitRow;
        xitCol = exitCol;
        playerImg = playerImg;
        exitImg = exitImg;
        playerRowLoc = strRow;
        playerColLoc = strCol;
        Multi = multi;
        context.fillStyle = "#000000";
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < cols; j++) {
                if (mazeData[i][j] == 1) {
                    context.fillRect(cellWidth * j, cellHeight * i,
                        cellWidth, cellHeight);
                }
            }
        }
        context.drawImage(playerImg, playerColLoc * cellWidth, playerRowLoc * cellHeight, cellWidth, cellHeight);
        context.drawImage(exitImg, xitCol * cellWidth, xitRow * cellHeight, cellWidth, cellHeight);
        if (enabled) {
            document.addEventListener("keydown", move, false);
        }
        if (multi) {
            Rival = rival;
        }
    };
    function movePlayer(newRowLoc, newColLoc) {
        context.fillStyle = "#ffffff";
        context.fillRect(playerColLoc * cellWidth, playerRowLoc * cellHeight, cellWidth, cellHeight);
        context.drawImage(playerImg, newColLoc * cellWidth, newRowLoc * cellHeight, cellWidth, cellHeight);
        playerColLoc = newColLoc;
        playerRowLoc = newRowLoc;
    }
    function move(event) {

        switch (event.keyCode) {
            case 37:
                if (mazeDat[playerRowLoc][playerColLoc - 1] == 0) {
                    movePlayer(playerRowLoc, playerColLoc - 1);
                    if (Multi) {
                        messagesHub.messagesHub.server.sendMessage(sessionStorage.getItem("loggedInUser"),Rival,"left" );
                    }
                }

                break;
            case 38:
                if (mazeDat[playerRowLoc - 1][playerColLoc] == 0) {
                    movePlayer(playerRowLoc - 1, playerColLoc);
                    if (Multi) {
                        messagesHub.messagesHub.server.sendMessage(sessionStorage.getItem("loggedInUser"), Rival, "up");
                    }
                }
                break;
            case 39:
                if (mazeDat[playerRowLoc][playerColLoc + 1] == 0) {
                    movePlayer(playerRowLoc, playerColLoc + 1);
                    if (Multi) {
                        messagesHub.messagesHub.server.sendMessage(sessionStorage.getItem("loggedInUser"), Rival, "down");
                    }
                }
                break;
            case 40:
                if (mazeDat[playerRowLoc + 1][playerColLoc] == 0) {
                    movePlayer(playerRowLoc + 1, playerColLoc);
                    if (Multi) {
                        messagesHub.messagesHub.server.sendMessage(sessionStorage.getItem("loggedInUser"), Rival, "right");
                    }
                }
                break;
        }
        if (playerRowLoc == xitRow && playerColLoc == xitCol) {
            document.removeEventListener('keydown', movePlayer);
            if (Multi) {
                var apiUrl = "../api//Registry/SetRank/" + sessionStorage.getItem("loggedInUser")+ "/" + "Win";
                $.ajax({
                    method: "GET",
                    url: apiUrl
                }).done(function (maze) { });

            }
            new PNotify({
                title: '',
                text: 'you won!! =)',
                type: 'success',
                hide: false
            });
           
        }
    }
    $.fn.solveMaze = function (solution) {
        movePlayer(strRow, strCol);
        var i = 0;
        var x = setInterval(function () {

            switch (solution[i]) {
                case '0':
                    movePlayer(playerRowLoc, playerColLoc - 1);
                    break;
                case '1':
                    movePlayer(playerRowLoc, playerColLoc + 1);
                    break;
                case '2':
                    movePlayer(playerRowLoc - 1, playerColLoc);
                    break;
                case '3':
                    movePlayer(playerRowLoc + 1, playerColLoc);
                    break;
            }
            i++;
        }, 200);
    };



    $.fn.rivalMove = function (direction) {

        switch (direction) {
            case "left":
                if (mazeDat[playerRowLoc][playerColLoc - 1] == 0) {
                    movePlayer(playerRowLoc, playerColLoc - 1);
                }

                break;
            case "up":
                if (mazeDat[playerRowLoc - 1][playerColLoc] == 0) {
                    movePlayer(playerRowLoc - 1, playerColLoc);
                }
                break;
            case "right":
                if (mazeDat[playerRowLoc][playerColLoc + 1] == 0) {
                    movePlayer(playerRowLoc, playerColLoc + 1);
                }
                break;
            case "down":
                if (mazeDat[playerRowLoc + 1][playerColLoc] == 0) {
                    movePlayer(playerRowLoc + 1, playerColLoc);
                }
                break;
            default:
                break;
        }
        if (playerRowLoc == xitRow && playerColLoc == xitCol) {
            
                var apiUrl = "../api//Registry/SetRank/" + sessionStorage.getItem("loggedInUser") + "/" + "lose";
                $.ajax({
                    method: "GET",
                    url: apiUrl
                }).done(function (maze) { });

            
                new PNotify({
                    title: '',
                    text: 'you Lost =(',
                    type: 'success',
                    hide: false
                });
        }

    };

})(jQuery); 
