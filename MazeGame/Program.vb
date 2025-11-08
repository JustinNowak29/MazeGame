Imports System
Imports System.Security.Cryptography.X509Certificates
Imports Thread

Module Program



    Sub Main()
        Dim Maze(9, 9) As String
        Dim Player As String = "O"
        Dim PlayerPos() As Integer = {9, 1}
        Dim PelletCount As Integer = -1

        InitMaze(Maze, Player)
        While PelletCount <> 0
            MovePlayer(Player, Maze, PlayerPos)
            CheckForWin(Maze, PelletCount)
        End While
    End Sub

    Sub InitMaze(ByRef Maze(,) As String, ByVal Player As String)

        For Row As Integer = 0 To 9
            For Col As Integer = 0 To 9
                Maze(Row, Col) = "." ' Will add pellets into maze first
                'Below code will overwrite code with walls
                Maze(0, Col) = "X"
                Maze(9, Col) = "X"
                Maze(Col, 0) = "X"
                Maze(Col, 9) = "X"
            Next
        Next
        For x As Integer = 0 To 5
            Maze(7, x) = "X"
        Next
        For x As Integer = 1 To 7
            Maze(2, x) = "X"
        Next
        For x As Integer = 4 To 9
            Maze(x, 7) = "X"
        Next
        For x As Integer = 4 To 5
            For y As Integer = 2 To 5
                Maze(x, y) = "X"
            Next
        Next

        Maze(9, 1) = Player
    End Sub

    Sub DisplayMaze(ByRef Maze(,) As String)

        For Row As Integer = 0 To 9
            For Col As Integer = 0 To 9
                Console.Write(Maze(Row, Col) & " ")
            Next
            Console.WriteLine()
        Next

    End Sub

    Sub MovePlayer(ByVal Player As String, ByRef Maze(,) As String, ByRef PlayerPos() As Integer)

        DisplayMaze(Maze)

        Dim Choice As String
        Console.Write("Press WASD to move: ")
        Choice = Console.ReadKey().KeyChar.ToString()

        Console.WriteLine()

        Dim NewPos() As Integer 'Will determine what is ahead of the current position, depending on what direction you will go to
        ' For example, if you press D, it will tell you what will be at new position, could be a Wall or Pellet

        Select Case Choice.ToUpper()
            Case "W"
                NewPos = {PlayerPos(0) - 1, PlayerPos(1)} 'Position that is 1 up from current
                CheckForX(Maze, PlayerPos, Player, NewPos)
            Case "A"
                NewPos = {PlayerPos(0), PlayerPos(1) - 1} 'Position that is 1 left from current
                CheckForX(Maze, PlayerPos, Player, NewPos)
            Case "S"
                NewPos = {PlayerPos(0) + 1, PlayerPos(1)} 'Position that is 1 down from current
                CheckForX(Maze, PlayerPos, Player, NewPos)
            Case "D"
                NewPos = {PlayerPos(0), PlayerPos(1) + 1} 'Position that is 1 right from current
                CheckForX(Maze, PlayerPos, Player, NewPos)
            Case Else
                Console.WriteLine("Wrong input, try again!")
                Threading.Thread.Sleep(1000)
        End Select


        Console.Clear()

    End Sub

    Sub CheckForX(ByRef Maze(,) As String, ByRef PlayerPos() As Integer, ByVal Player As String, ByVal NewPos() As Integer)

        If Maze(NewPos(0), NewPos(1)) = "X" Then 'If there is a wall in the next position
            Console.WriteLine("Cannot hit the wall! Back to start")
            Threading.Thread.Sleep(1000)
            Maze(PlayerPos(0), PlayerPos(1)) = " " 'Will replace current position as blank
            Maze(9, 1) = Player 'Sends player back to the start
            PlayerPos = {9, 1}
        Else 'If there is pellet in next position
            Maze(NewPos(0), NewPos(1)) = Player 'Overwrite pellet with Player "O"
            Maze(PlayerPos(0), PlayerPos(1)) = " " 'Will replace current position as blank
            PlayerPos = NewPos 'Since a move has been made, replaces new validated position with old.
        End If

    End Sub

    Sub CheckForWin(ByVal Maze(,) As String, ByRef PelletCount As Integer)

        PelletCount = 0
        For Row As Integer = 0 To 9
            For Col As Integer = 0 To 9
                If Maze(Row, Col) = "." Then
                    PelletCount += 1
                End If
            Next
        Next

        Console.WriteLine($"Pellet Count: {PelletCount}")

        If PelletCount = 0 Then
            Console.WriteLine("You win!")
        End If

    End Sub

End Module
