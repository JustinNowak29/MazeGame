Imports System
Imports System.ComponentModel.Design
Imports System.Security.Cryptography.X509Certificates
Imports Thread

Module Program

    Dim Maze(15, 15) As String
    Dim Player As String = "O"


    Sub Main()
        Dim Running As Boolean = True
        Dim StartingPos() As Integer = New Integer(1) {} 'size 2: indices 0 and 1
        Dim PlayerPos() As Integer = New Integer(1) {}
        Dim PelletCount As Integer = -1
        Dim Choice As Integer

        While Running

            Console.Clear()
            Console.Write("What would you like to do?" & Environment.NewLine &
                          "1. Fixed Maze" & Environment.NewLine &
                          "2. Random Maze" & Environment.NewLine &
                          "3. Exit" & Environment.NewLine &
                          "Enter option here: ")
            Choice = Console.ReadLine
            Select Case Choice
                Case 1 'Fixed Maze
                    FixedMaze(StartingPos, PlayerPos, PelletCount)
                    While PelletCount <> 0
                        MovePlayer(StartingPos, PlayerPos, PelletCount)
                        CheckForWin(PelletCount)
                    End While

                Case 2 'Random Maze
                    RandomiseMaze(StartingPos, PlayerPos, PelletCount)
                    While PelletCount <> 0
                        MovePlayer(StartingPos, PlayerPos, PelletCount)
                        CheckForWin(PelletCount)
                    End While

                Case 3 'Exit
                    Running = False
                    Exit While
                Case Else ' If invalid input
                    Console.WriteLine("Invalid input, try again!")
                    Threading.Thread.Sleep(1000)
            End Select

            Console.Clear()


        End While

    End Sub

    Sub PlayerPlacement(ByRef StartingPos() As Integer, ByRef PlayerPos() As Integer)

        Dim Rnd As New Random()

        While True
            Dim RandRow As Integer = Rnd.Next(1, 15)
            Dim RandCol As Integer = Rnd.Next(1, 15)
            If Maze(RandRow, RandCol) = "." Then
                StartingPos(0) = RandRow
                StartingPos(1) = RandCol
                Maze(StartingPos(0), StartingPos(1)) = Player
                Exit While
            End If
        End While

        PlayerPos = StartingPos

    End Sub


    Sub FixedMaze(ByRef StartingPos() As Integer, ByRef PlayerPos() As Integer, ByRef PelletCount As Integer)

        For Row As Integer = 0 To 9
            For Col As Integer = 0 To 9
                Maze(Row, Col) = "." 'Will add pellets into maze first
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

        PlayerPlacement(StartingPos, PlayerPos)

        Console.SetCursorPosition(0, 0) ' Move cursor to top-left

        ' Count the total pellets
        PelletCount = 0
        For r As Integer = 0 To 9
            For c As Integer = 0 To 9
                If Maze(r, c) = "." Then
                    PelletCount += 1
                End If
            Next
        Next

    End Sub

    Sub RandomiseMaze(ByRef StartingPos() As Integer, ByRef PlayerPos() As Integer, ByRef PelletCount As Integer) 'This sub will randomise the maze layout each time the game is run

        Dim Rnd As New Random()
        Dim PelletLineCount As Integer = Rnd.Next(20, 25)

        For Row As Integer = 0 To 15
            For Col As Integer = 0 To 15
                Maze(Row, Col) = "X" 'Will add pellets into maze first
            Next
        Next

        Dim LastLine(14, 1) As Integer
        Dim LastCount As Integer = 0

        For x As Integer = 1 To PelletLineCount

            Dim CurrentLine(14, 1) As Integer
            Dim IntersectionFound As Boolean = False
            Dim CurrentCount As Integer = 0

            Do
                CurrentCount = 0
                Dim TwoDigitConfirm = 0

                Dim AxisDeterminer As Integer = Rnd.Next(1, 3) '1 = Horizontal, 2 = Vertical
                Dim StartPosition As Integer = Rnd.Next(1, 15) 'From 1 to 8 so there is space for wall thickness. Will either be row or column depending on axis
                Dim StartLength As Integer = Rnd.Next(1, 15)
                Dim EndLength As Integer = Rnd.Next(1, 15)

                If EndLength < StartLength Then 'Ensures StartLength is always less than EndLength in For loop
                    Dim Temp As Integer = StartLength
                    StartLength = EndLength
                    EndLength = Temp
                End If

                If AxisDeterminer = 1 Then 'Horizontal Line
                    For col As Integer = StartLength To EndLength
                        CurrentLine(CurrentCount, 0) = StartPosition
                        CurrentLine(CurrentCount, 1) = col
                        'Console.Write("(" & CurrentLine(CurrentCount, 0) & "," & (CurrentLine(CurrentCount, 1)) & ") ")
                        CurrentCount += 1


                    Next
                Else 'Vertical Line
                    For row As Integer = StartLength To EndLength
                        CurrentLine(CurrentCount, 0) = row
                        CurrentLine(CurrentCount, 1) = StartPosition
                        'Console.Write("(" & CurrentLine(CurrentCount, 0) & "," & (CurrentLine(CurrentCount, 1)) & ") ")
                        CurrentCount += 1
                    Next
                End If

                'Console.ReadLine()

                Dim IntersectionPoint(1) As Integer 'Where the two lines intersect

                For i As Integer = 0 To LastCount - 1 '<--- we are checking If the both the x And y coords are the same In both lists.
                    For j As Integer = 0 To CurrentCount - 1
                        If LastLine(i, 0) = CurrentLine(j, 0) AndAlso LastLine(i, 1) = CurrentLine(j, 1) Then
                            TwoDigitConfirm += 1 'x and y both match
                            IntersectionPoint(0) = CurrentLine(j, 0)
                            IntersectionPoint(1) = CurrentLine(j, 1)

                        End If
                    Next
                Next


                If TwoDigitConfirm >= 1 Or (LastCount = 0 And x = 1) Then
                    IntersectionFound = True

                    For squares As Integer = 0 To CurrentCount - 1

                        If CurrentLine(squares, 0) = IntersectionPoint(0) And CurrentLine(squares, 1) = IntersectionPoint(1) Then
                            Maze(CurrentLine(squares, 0), CurrentLine(squares, 1)) = "#" 'Intersection point
                        ElseIf Maze(CurrentLine(squares, 0), CurrentLine(squares, 1)) = "#" Then
                            'Do nothing, keep as intersection
                        Else
                            Maze(CurrentLine(squares, 0), CurrentLine(squares, 1)) = "." 'Pellet pathway

                        End If

                    Next

                    For k As Integer = 0 To CurrentCount - 1
                        LastLine(k, 0) = CurrentLine(k, 0)
                        LastLine(k, 1) = CurrentLine(k, 1)
                    Next
                    LastCount = CurrentCount
                End If

            Loop Until IntersectionFound = True

            'Code below will add walls around pellets if there is empty space adjacent to them
            ' CHANGE LATER: Code messy
            Dim EmptySpaceCount As Integer


            Console.SetCursorPosition(0, 0) ' Move cursor to top-left
            DisplayMaze()
            Threading.Thread.Sleep(5)


            For row As Integer = 1 To 14
                For col As Integer = 1 To 14
                    If Maze(row, col) = "." Then

                        'DisplayMaze()
                        EmptySpaceCount = 0

                        'Check all 4 directions
                        If Maze(row, col - 1) = "." And Maze(row, col - 1) <> "#" Then EmptySpaceCount += 1
                        If Maze(row, col + 1) = "." And Maze(row, col + 1) <> "#" Then EmptySpaceCount += 1
                        If Maze(row - 1, col) = "." And Maze(row - 1, col) <> "#" Then EmptySpaceCount += 1
                        If Maze(row + 1, col) = "." And Maze(row + 1, col) <> "#" Then EmptySpaceCount += 1
                        If Maze(row - 1, col - 1) = "." And Maze(row - 1, col - 1) <> "#" Then EmptySpaceCount += 1
                        If Maze(row - 1, col + 1) = "." And Maze(row - 1, col + 1) <> "#" Then EmptySpaceCount += 1
                        If Maze(row + 1, col - 1) = "." And Maze(row + 1, col - 1) <> "#" Then EmptySpaceCount += 1
                        If Maze(row + 1, col + 1) = "." And Maze(row + 1, col + 1) <> "#" Then EmptySpaceCount += 1

                        If EmptySpaceCount = 4 Then
                            Maze(row, col) = "#"
                        ElseIf EmptySpaceCount >= 7 Then
                            Maze(row, col) = "X"
                            'Maze(row, col) = "X" 'Convert pellet to wall if 3 or more sides are empty space)
                        End If

                        If Maze(row, col) = "#" Then
                            Maze(row, col) = "."
                        End If
                    End If
                Next
            Next

            'Console.ReadLine()


        Next


        For i As Integer = 0 To 15
            Maze(0, i) = "X"
            Maze(15, i) = "X"
            Maze(i, 0) = "X"
            Maze(i, 15) = "X"
        Next

        ' Count the total pellets
        PelletCount = 0
        For row As Integer = 1 To 14
            For col As Integer = 1 To 14

                ' Count surrounding empty spaces (non-walls)
                Dim EmptySpaceCount As Integer = 0

                ' Check all 8 directions
                If Maze(row, col - 1) <> "X" Then EmptySpaceCount += 1
                If Maze(row, col + 1) <> "X" Then EmptySpaceCount += 1
                If Maze(row - 1, col) <> "X" Then EmptySpaceCount += 1
                If Maze(row + 1, col) <> "X" Then EmptySpaceCount += 1
                If Maze(row - 1, col - 1) <> "X" Then EmptySpaceCount += 1
                If Maze(row - 1, col + 1) <> "X" Then EmptySpaceCount += 1
                If Maze(row + 1, col - 1) <> "X" Then EmptySpaceCount += 1
                If Maze(row + 1, col + 1) <> "X" Then EmptySpaceCount += 1

                ' Apply smoothing based on nearby open space
                If EmptySpaceCount <= 1 Then
                    ' Surrounded by walls → fill in
                    Maze(row, col) = "X"

                ElseIf EmptySpaceCount >= 7 Then
                    ' Surrounded by open space → fill in to reduce large caves
                    Maze(row, col) = "X"

                ElseIf EmptySpaceCount = 4 Or EmptySpaceCount = 5 Then
                    ' Balanced areas (keep as path)
                    Maze(row, col) = "."
                End If

                If Maze(row, col) = "#" Then Maze(row, col) = "."
                If Maze(row, col) = "." Then PelletCount += 1

            Next

        Next

        PlayerPlacement(StartingPos, PlayerPos)

    End Sub
    Sub DisplayMaze()

        Console.SetCursorPosition(0, 0) ' Move cursor to top-left

        For Row As Integer = 0 To 15
            For Col As Integer = 0 To 15

                If Row = 0 Or Row = 15 Or Col = 0 Or Col = 15 Then
                    Console.ForegroundColor = ConsoleColor.DarkGray   ' Borders in dark blue
                ElseIf Maze(Row, Col) = "X" Then
                    Console.ForegroundColor = ConsoleColor.Gray ' Internal walls in white
                End If

                If Maze(Row, Col) = Player Then
                    Console.ForegroundColor = ConsoleColor.DarkGreen ' Player in green
                ElseIf Maze(Row, Col) = "#" Then
                    Console.ForegroundColor = ConsoleColor.DarkYellow
                ElseIf Maze(Row, Col) = "." Then
                    Console.ForegroundColor = ConsoleColor.Yellow ' Pellets in yellow

                End If


                Console.Write(Maze(Row, Col) & " ")
            Next
            Console.WriteLine()
        Next

        Console.ResetColor()

    End Sub


    Sub MovePlayer(ByRef StartingPos() As Integer, ByRef PlayerPos() As Integer, ByRef PelletCount As Integer)

        DisplayMaze()

        Dim Choice As String
        Console.Write("Press WASD to move: ")
        Choice = Console.ReadKey().KeyChar.ToString()
        Console.WriteLine()

        Dim NewPos() As Integer 'Will determine what is ahead of the current position, depending on what direction you will go to
        ' For example, if you press D, it will tell you what will be at new position, could be a Wall or Pellet
        Select Case Choice.ToUpper()
            Case "W"
                NewPos = {PlayerPos(0) - 1, PlayerPos(1)} 'Position that is 1 up from current
                CheckForX(StartingPos, PlayerPos, NewPos, PelletCount)
            Case "A"
                NewPos = {PlayerPos(0), PlayerPos(1) - 1} 'Position that is 1 left from current
                CheckForX(StartingPos, PlayerPos, NewPos, PelletCount)
            Case "S"
                NewPos = {PlayerPos(0) + 1, PlayerPos(1)} 'Position that is 1 down from current
                CheckForX(StartingPos, PlayerPos, NewPos, PelletCount)
            Case "D"
                NewPos = {PlayerPos(0), PlayerPos(1) + 1} 'Position that is 1 right from current
                CheckForX(StartingPos, PlayerPos, NewPos, PelletCount)
            Case Else
                Console.WriteLine("Wrong input, try again!")
                Threading.Thread.Sleep(1000)
        End Select

    End Sub

    Sub CheckForX(ByRef StartingPos() As Integer, ByRef PlayerPos() As Integer, ByRef NewPos() As Integer, ByRef PelletCount As Integer)

        If Maze(NewPos(0), NewPos(1)) = "X" Then 'If there is a wall in the next position
            Console.WriteLine("Cannot hit the wall! Back to start")
            Threading.Thread.Sleep(1000)
            Maze(PlayerPos(0), PlayerPos(1)) = " " 'Will replace current position as blank
            Maze(StartingPos(0), StartingPos(1)) = Player 'Sends player back to the start
            PlayerPos = StartingPos 'Since a move has been made, replaces new validated position with old.
            Console.Clear()

        Else 'If there is pellet in next position

            If Maze(NewPos(0), NewPos(1)) = "." Then
                PelletCount -= 1
            End If

            Maze(NewPos(0), NewPos(1)) = Player 'Overwrite pellet with Player "O"
            Maze(PlayerPos(0), PlayerPos(1)) = " " 'Will replace current position as blank
            PlayerPos = NewPos 'Since a move has been made, replaces new validated position with old.
        End If

    End Sub

    Sub CheckForWin(ByRef PelletCount As Integer)

        Console.WriteLine($"Pellet Count: {PelletCount}")

        If PelletCount = 0 Then
            Console.WriteLine("You win!")
            Threading.Thread.Sleep(1500)
        End If

    End Sub

End Module
