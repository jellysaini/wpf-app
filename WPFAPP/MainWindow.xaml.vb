Imports System.Data
Class MainWindow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Loaded

        BindEvents()

    End Sub


    Protected Sub BindEvents()
        Try

            Dim objService As New Service()
            Dim dt As New DataTable()
            dt = objService.GetAllEvents()
            cEvents.ItemsSource = dt.DefaultView
            cEvents.DisplayMemberPath = dt.Columns("Description").ToString()
            cEvents.SelectedValuePath = dt.Columns("EventID").ToString()


            ccEvents.ItemsSource = dt.DefaultView
            ccEvents.DisplayMemberPath = dt.Columns("Description").ToString()
            ccEvents.SelectedValuePath = dt.Columns("EventID").ToString()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        End Try

    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        Try

#Region "Validation"

            If txtPlayerID.Text = String.Empty Then
                tbError.Text = "Please Enter the Player ID."
                Return
            End If

            If cEvents.SelectedValue = String.Empty Then
                tbError.Text = "Please Enter the Event."
                Return
            End If


            If txtNOS.Text = String.Empty Then
                tbError.Text = "Please Enter the No Of Seat(s)."
                Return
            End If

            tbError.Text = ""

#End Region

            Dim playerID As String
            Dim eventID As String
            Dim noOfSeats As String

            playerID = txtPlayerID.Text
            eventID = cEvents.SelectedValue
            noOfSeats = txtNOS.Text

            Dim objService As New Service()
            Dim dt As New DataTable()

#Region "Three Point Check"
            Dim tcomCheck As String
            tcomCheck = ""
            Dim dtThree As New DataTable()
            dtThree = objService.GetThreeCheck(eventID)

            If dtThree.Rows.Count > 0 Then
                Dim openInvitation As String
                openInvitation = Convert.ToString(dtThree.Rows(0)("OpenInvitation"))
                If openInvitation = "1" Or openInvitation = "True" Then
                    'Do Nothing ...
                Else
                    Dim invitationList As String
                    invitationList = Convert.ToString(dtThree.Rows(0)("InvitationList"))
                    If invitationList = "1" Or invitationList = "True" Then
                        Dim dtInvite As New DataTable
                        dtInvite = objService.CompareInviteValues(eventID, playerID)
                        If Not dtInvite Is Nothing Then
                            If dtInvite.Rows.Count > 0 Then
                                'Do Nothing ...
                            Else
                                dgSeats.ItemsSource = Nothing
                                MessageBox.Show("Validation Error.", "Error")
                                Return
                            End If
                        Else
                            dgSeats.ItemsSource = Nothing
                            MessageBox.Show("Validation Error.", "Error")
                            Return
                        End If
                    Else
                        Dim tStart As String
                        Dim tEnd As String

                        tStart = Convert.ToString(dtThree.Rows(0)("TStart"))
                        tEnd = Convert.ToString(dtThree.Rows(0)("TEnd"))

                        Dim dtCheck As New DataTable()
                        dtCheck = objService.CompareTValues(tStart, tEnd, playerID)
                        If Not dtCheck Is Nothing Then
                            If dtCheck.Rows.Count > 0 Then
                                tcomCheck = "true"
                            Else
                                dgSeats.ItemsSource = Nothing
                                MessageBox.Show("Validation Error.", "Error")
                                Return
                            End If
                        Else
                            dgSeats.ItemsSource = Nothing
                            MessageBox.Show("Validation Error.", "Error")
                            Return
                        End If
                    End If
                End If
            Else
                dgSeats.ItemsSource = Nothing
                MessageBox.Show("Validation Error.", "Error")
                Return
            End If

#End Region

            'If tcomCheck = "true" Then
            '    dt = objService.GetSearchDataAll(playerID, eventID, noOfSeats)
            'Else
            dt = objService.GetSearchData(playerID, eventID, noOfSeats)
            'End If


            If dt.Rows.Count > 0 Then
                dgSeats.ItemsSource = dt.DefaultView
                dgSeats.DataContext = dt.DefaultView
            Else
                dgSeats.ItemsSource = Nothing
                MessageBox.Show("No data found.", "Error")
            End If

        Catch ex As Exception
            dgSeats.ItemsSource = Nothing
            MessageBox.Show(ex.Message, "Error")

        End Try


    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        Try

#Region "Validation"

            If txtPlayerID.Text = String.Empty Then
                tbError.Text = "Please Enter the Player ID."
                Return
            End If

            If cEvents.SelectedValue = String.Empty Then
                tbError.Text = "Please Enter the Event."
                Return
            End If


            If txtNOS.Text = String.Empty Then
                tbError.Text = "Please Enter the No Of Seat(s)."
                Return
            End If

            If DataGridSelectedValue() = "" Then
                tbError.Text = "Please selecte one row from the grid."
                Return
            End If

            tbError.Text = ""
            tbError.Text = ""

#End Region

            Dim playerID As String
            Dim eventID As String
            Dim noOfSeats As String
            Dim seatingID As String


            playerID = txtPlayerID.Text
            eventID = cEvents.SelectedValue
            noOfSeats = txtNOS.Text
            seatingID = DataGridSelectedValue()


            Dim objService As New Service()
            objService.AddRes(playerID, eventID, noOfSeats, seatingID)

            dgSeats.ItemsSource = Nothing

            MessageBox.Show("Data saved successfully.", "Success")

        Catch ex As Exception

            If ex.Message.Contains("PK_Reservations") Then
                Dim confirmResult = MessageBox.Show("Same data already exists in database. Would you like to update ?", "Confirm!", MessageBoxButton.YesNo)
                If Convert.ToString(confirmResult) = "6" Then
                    Dim playerID As String
                    Dim eventID As String
                    Dim noOfSeats As String
                    Dim seatingID As String


                    playerID = txtPlayerID.Text
                    eventID = cEvents.SelectedValue
                    noOfSeats = txtNOS.Text
                    seatingID = DataGridSelectedValue()


                    Dim objService As New Service()
                    objService.UpdateRes(playerID, eventID, noOfSeats, seatingID)

                    MessageBox.Show("Data updated successfully.", "Success")
                End If
            Else
                MessageBox.Show(ex.Message, "Error")
            End If

        End Try
    End Sub

    Public Function DataGridSelectedValue() As String
        Dim selectedValue As String
        selectedValue = ""
        Try
            For Each row As DataRowView In dgSeats.SelectedItems
                Dim MyRow As System.Data.DataRow = CType(row.Row, System.Data.DataRow)
                selectedValue = MyRow("SID").ToString()
            Next
            Return selectedValue
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function DataGridSelectedValueForCheck() As String
        Dim selectedValue As String
        selectedValue = ""
        Try
            For Each row As DataRowView In dgcSeats.SelectedItems
                Dim MyRow As System.Data.DataRow = CType(row.Row, System.Data.DataRow)
                selectedValue = MyRow("SID").ToString()
            Next
            Return selectedValue
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub BtncSearch_Click(sender As Object, e As RoutedEventArgs) Handles btncSearch.Click
        Try

#Region "Validation"

            If txtcPlayerID.Text = String.Empty Then
                tbcError.Text = "Please Enter the Player ID."
                Return
            End If

            If ccEvents.SelectedValue = String.Empty Then
                tbcError.Text = "Please Enter the Event."
                Return
            End If


            If txtcNOS.Text = String.Empty Then
                tbcError.Text = "Please Enter the No Of Seat(s)."
                Return
            End If

            tbcError.Text = ""

#End Region

            Dim playerID As String
            Dim eventID As String
            Dim noOfSeats As String

            playerID = txtcPlayerID.Text
            eventID = ccEvents.SelectedValue
            noOfSeats = txtcNOS.Text

            Dim objService As New Service()
            Dim dt As New DataTable()

#Region "Three Point Check"
            Dim tcomCheck As String
            tcomCheck = ""
            Dim dtThree As New DataTable()
            dtThree = objService.GetThreeCheck(eventID)

            If dtThree.Rows.Count > 0 Then
                Dim openInvitation As String
                openInvitation = Convert.ToString(dtThree.Rows(0)("OpenInvitation"))
                If openInvitation = "1" Or openInvitation = "True" Then
                    'Do Nothing ...
                Else
                    Dim invitationList As String
                    invitationList = Convert.ToString(dtThree.Rows(0)("InvitationList"))
                    If invitationList = "1" Or invitationList = "True" Then
                        Dim dtInvite As New DataTable
                        dtInvite = objService.CompareInviteValues(eventID, playerID)
                        If Not dtInvite Is Nothing Then
                            If dtInvite.Rows.Count > 0 Then
                                'Do Nothing ...
                            Else
                                dgSeats.ItemsSource = Nothing
                                MessageBox.Show("Validation Error.", "Error")
                                Return
                            End If
                        Else
                            dgSeats.ItemsSource = Nothing
                            MessageBox.Show("Validation Error.", "Error")
                            Return
                        End If
                    Else
                        Dim tStart As String
                        Dim tEnd As String

                        tStart = Convert.ToString(dtThree.Rows(0)("TStart"))
                        tEnd = Convert.ToString(dtThree.Rows(0)("TEnd"))

                        Dim dtCheck As New DataTable()
                        dtCheck = objService.CompareTValues(tStart, tEnd, playerID)
                        If Not dtCheck Is Nothing Then
                            If dtCheck.Rows.Count > 0 Then
                                tcomCheck = "true"
                            Else
                                dgSeats.ItemsSource = Nothing
                                MessageBox.Show("Validation Error.", "Error")
                                Return
                            End If
                        Else
                            dgSeats.ItemsSource = Nothing
                            MessageBox.Show("Validation Error.", "Error")
                            Return
                        End If
                    End If
                End If
            Else
                dgSeats.ItemsSource = Nothing
                MessageBox.Show("Validation Error.", "Error")
                Return
            End If

#End Region



            'If tcomCheck = "true" Then
            '    dt = objService.GetSearchDataAll(playerID, eventID, noOfSeats)
            'Else
            dt = objService.GetSearchData(playerID, eventID, noOfSeats)
            'End If

            If dt.Rows.Count > 0 Then
                dgSeats.ItemsSource = dt.DefaultView
                dgcSeats.DataContext = dt.DefaultView
            Else
                dgSeats.ItemsSource = Nothing
                MessageBox.Show("No data found.", "Error")
            End If

        Catch ex As Exception

            dgSeats.ItemsSource = Nothing
            MessageBox.Show(ex.Message, "Error")

        End Try
    End Sub

    Private Sub BtnCheck_Click(sender As Object, e As RoutedEventArgs) Handles btnCheck.Click
        Try

#Region "Validation"

            If txtcPlayerID.Text = String.Empty Then
                tbcError.Text = "Please Enter the Player ID."
                Return
            End If

            If ccEvents.SelectedValue = String.Empty Then
                tbcError.Text = "Please Enter the Event."
                Return
            End If


            If txtcNOS.Text = String.Empty Then
                tbcError.Text = "Please Enter the No Of Seat(s)."
                Return
            End If

            If DataGridSelectedValueForCheck() = "" Then
                tbcError.Text = "Please selecte one row from the grid."
                Return
            End If

            tbcError.Text = ""


#End Region

            Dim playerID As String
            Dim eventID As String
            Dim noOfSeats As String
            Dim seatingID As String


            playerID = txtcPlayerID.Text
            eventID = ccEvents.SelectedValue
            noOfSeats = txtcNOS.Text
            seatingID = DataGridSelectedValueForCheck()


            Dim objService As New Service()
            objService.UpdateCheckValue(playerID, eventID, noOfSeats, seatingID)

            MessageBox.Show("Data updated successfully.", "Success")

        Catch ex As Exception

            MessageBox.Show(ex.Message, "Error")

        End Try
    End Sub
End Class
