$("#StrtBtn").click(function () {
    var apiUrl = "../api/Single/" + $("#mazeName").val() + "/" + $("#rows").val() + "/" + $("#cols").val();
    /*$.getJSON(apiUrl)*/$.ajax({
        method: "GET",
        url: apiUrl
    }).done(function (maze) {

        var rows = maze.Rows;
        var cols = maze.Cols;   
        var startRow = maze.Start.Row;
        var startCol = maze.Start.Col;
        var exitRow = maze.End.Row;
        var exitCol = maze.End.Col;

        var mazeData = new Array(rows);
        for (var i = 0; i < rows; i++) {
            mazeData[i] = new Array(cols);
        }

        var fromStr = maze.Maze.split("");

        for (var j = 0; j < rows; j++) {
            for (var k = 0; k < cols; k++) {
                mazeData[j][k] = fromStr[(j * cols) + k];
            }
        }

        var mazeBoard = $("#mazeCanvas").mazeBoard(mazeData, startRow, startCol, exitRow, exitCol,true);
        document.title = $("#mazeName").val();

    });
});

$("#SlvBtn").click(function () {
    var algo = $("#dd").text();
    if (algo == "Bfs ") {
        algo = 1;
    } else {
        algo = 0;
    }
    var apiUrl = "../api/Single/" + $("#mazeName").val() + "/" + algo;
    $.getJSON(apiUrl).done(function (solution) {
        var fromSolution = solution.Solution.split("");
        $("#mazeCanvas").solveMaze(fromSolution);
    });
});

$(".dropdown-menu li a").click(function () {
    $(this).parents(".dropdown").find('.btn').html($(this).text() + ' <span class="caret"></span>');
    $(this).parents(".dropdown").find('.btn').val($(this).data('value'));
});
/*});*/
//{"Name":"my","Maze":"0001001110000101101000000","Rows":5,"Cols":5,"Start":{"Row":0,"Col":2},"End":{"Row":2,"Col":2}}