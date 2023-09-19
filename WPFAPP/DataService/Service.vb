
Imports System.Data
Imports System.Data.SqlClient
Public Class Service
    Public Function GetAllEvents() As DataTable
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim cmd As New SqlCommand("select distinct EventID,Description from Events;", con)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet()
        da.Fill(ds)
        Dim dt As New DataTable()
        dt = ds.Tables(0)
        Return dt
    End Function


    Public Function GetSearchData(ByVal playerID As String, ByVal eventID As String, ByVal noOfSeats As String) As DataTable

        Dim dt As New DataTable()
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim cmd As New SqlCommand("select * from Seatings where  LEFT(CONVERT(DATETIME,Buisnessdate,103),12)>=LEFT(CONVERT(DATETIME,GetDate(),103),12) AND EventID='" + eventID + "'", con)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet()
        da.Fill(ds)
        dt = ds.Tables(0)
        Return dt

    End Function

    'Public Function GetSearchDataAll(ByVal playerID As String, ByVal eventID As String, ByVal noOfSeats As String) As DataTable

    '    Dim dt As New DataTable()
    '    Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
    '    If con.State = ConnectionState.Closed Then
    '        con.Open()
    '    End If
    '    Dim cmd As New SqlCommand("select * from Seatings where  EventID='" + eventID + "'", con)
    '    Dim da As New SqlDataAdapter(cmd)
    '    Dim ds As New DataSet()
    '    da.Fill(ds)
    '    dt = ds.Tables(0)
    '    Return dt

    'End Function

    Public Function CompareTValues(ByVal tStart As String, ByVal tEnd As String, ByVal playerID As String) As DataTable

        Dim dt As New DataTable()
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim cmd As New SqlCommand("select * from Pc_view Where T BETWEEN " & tStart & " AND " & tEnd & " AND ID=" & playerID & ";", con)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet()
        da.Fill(ds)
        dt = ds.Tables(0)
        Return dt

    End Function

    Public Function CompareInviteValues(ByVal eventID As String, ByVal playerID As String) As DataTable

        Dim dt As New DataTable()
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim cmd As New SqlCommand("SELECT * FROM Invites WHERE  EventID='" & eventID & "' AND ID=" & playerID & ";", con)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet()
        da.Fill(ds)
        dt = ds.Tables(0)
        Return dt

    End Function

    Public Function GetThreeCheck(ByVal eventID As String) As DataTable

        Dim dt As New DataTable()
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim cmd As New SqlCommand("select * from Events where EventID='" + eventID + "'", con)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet()
        da.Fill(ds)
        dt = ds.Tables(0)
        Return dt

    End Function

    Public Sub AddRes(ByVal playerID As String, ByVal eventID As String, ByVal noOfSeats As String, seatingID As String)
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        Dim StrInsert As String = "INSERT INTO Reserve(EventID,ID,NOS,SID,CheckIn)" & " VALUES (" & "'" & eventID & "'," & "'" & playerID & "'," & "'" & noOfSeats & "'," & "'" & seatingID & "',0)"
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim SqlCmd As New SqlCommand
        SqlCmd = New SqlCommand(StrInsert, con)
        SqlCmd.ExecuteNonQuery()
        con.Close()
    End Sub

    Public Sub UpdateCheckValue(ByVal playerID As String, ByVal eventID As String, ByVal noOfSeats As String, seatingID As String)
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        Dim StrUpdate As String = "UPDATE Reserve Set CheckIn=1 WHERE EventID='" & eventID & "' AND SID='" & seatingID & "' AND ID='" & playerID & "' AND NOS='" & noOfSeats & "'"
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim SqlCmd As New SqlCommand
        SqlCmd = New SqlCommand(StrUpdate, con)
        SqlCmd.ExecuteNonQuery()
        con.Close()
    End Sub

    Public Sub UpdateRes(ByVal playerID As String, ByVal eventID As String, ByVal noOfSeats As String, seatingID As String)
        Dim con As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("ConString").ConnectionString)
        Dim StrUpdate As String = "UPDATE Reserve Set EventID='" & eventID & "',ID='" & playerID & "', NOS='" & noOfSeats & "',SID='" & seatingID & "' WHERE EventID='" & eventID & "' AND SID='" & seatingID & "'"
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
        Dim SqlCmd As New SqlCommand
        SqlCmd = New SqlCommand(StrUpdate, con)
        SqlCmd.ExecuteNonQuery()
        con.Close()
    End Sub
End Class
