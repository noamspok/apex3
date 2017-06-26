$("#BtnStrt").click(function () {
    var mazeName = $("#mazeName").val();
    var r = $("#rows").val();
    var c = $("#cols").val();
    var apiUrl = "../api/Multi/";
    var obj = {
        Name: mazeName,
        Rows: r,
        Cols: c
    };
    $.post(apiUrl, obj).done(function (maze) {
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
        //need to change to operate only in join button
        $("#mazeCanvas").mazeBoard(mazeData, startRow, startCol, exitRow, exitCol, true);
        $("#rivalsMazeCanvas").mazeBoard(mazeData, startRow, startCol, exitRow, exitCol, false);

    });
});