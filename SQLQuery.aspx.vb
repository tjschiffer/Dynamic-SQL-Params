Option Strict On
Option Explicit On
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Collections.Generic
Imports System.Web.UI.WebControls
Imports System.Diagnostics

Partial Class _Default
    Inherits System.Web.UI.Page
End Class
Public Module SamplingAssistant
    Function sqlTable(ByVal tTable1 As Table, queryString1 As NameValueCollection) As Table
        Dim count As New Integer
        Dim queryString As New NameValueCollection(queryString1)
        If Integer.TryParse(queryString("Count"), count) Then Else count = 50 ' Defualt count to 50 if no value is specified
        If queryString.AllKeys.Contains("Count") Then queryString.Remove("Count")

        'Connect to the SQL Server that contains all the NOVA Sample information
        Dim sqlCon As New System.Data.SqlClient.SqlConnection
        sqlCon.ConnectionString = "Data Source=TJSDESKTOP\SQLEXPRESS;Persist Security Info=True;User ID=SQLReader;Password=hunter2"

        Dim sqlText As New StringBuilder(System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath + "assets\sql\FL Insurance Sample.sql"))

        Dim sqlCmdHeaders As New SqlClient.SqlCommand(sqlText.ToString, sqlCon)
        sqlCmdHeaders.Parameters.Add("@Count", SqlDbType.Int, 5).Value = 0

        sqlCon.Open()
        Dim sqlReaderHeaders As System.Data.SqlClient.SqlDataReader = sqlCmdHeaders.ExecuteReader()

        'Get the Headers as returned by the SQL Server
        Dim columnNames As New List(Of String)
        For i As Integer = 0 To sqlReaderHeaders.FieldCount - 1
            columnNames.Add(sqlReaderHeaders.GetName(i).ToString)
        Next i
        sqlReaderHeaders.Close()

        Dim rgx As New Regex("[^a-zA-Z0-9 -]")
        For Each whereClause As String In queryString.AllKeys
            Dim columnName As String = rgx.Replace(whereClause, "")
            If Not columnNames.Contains(columnName) Then
                Dim tH As New TableHeaderRow
                Dim cH As New TableHeaderCell
                cH.Controls.Add(New LiteralControl("You have entered a column name that is not returned by the query. " _
                                                + "Please see the following list of allowable column names."))
                tH.Cells.Add(cH)
                tTable1.Rows.Add(tH)
                Dim tH1 As New TableHeaderRow
                Dim cH1 As New TableHeaderCell
                cH1.Controls.Add(New LiteralControl("Column Name"))
                tH1.Cells.Add(cH1)
                tTable1.Rows.Add(tH1)
                For Each rowColumn As String In columnNames
                    Dim r As New TableRow
                    Dim c1 As New TableCell
                    c1.Controls.Add(New LiteralControl(rowColumn))
                    r.Cells.Add(c1)
                    tTable1.Rows.Add(r)
                Next rowColumn

                Return tTable1
                Exit Function
            End If
        Next whereClause

        Dim params As New Dictionary(Of String, String)
        Dim injection As New List(Of String)
        For Each whereClause As String In queryString.AllKeys
            Dim param As String = "@" + Strings.Replace(rgx.Replace(whereClause, ""), " ", "_")
            Dim val As String = queryString(whereClause)
            Dim comOperator As String
            If val.Contains("|") Then
                Dim comOperator1 As String() = Strings.Split(val, "|")
                comOperator = " " + Strings.Left(comOperator1(0), 2) + " "
                params.Add(param, Strings.Right(val, val.Length - val.IndexOf("|") - 1))
            ElseIf val.Contains(",") Then
                comOperator = " IN "
                Dim vals As String() = Strings.Split(val, ",")
                Dim paramList As New List(Of String)
                For i As Integer = 0 To vals.Count - 1
                    Dim paramName As String = "@" + param + i.ToString
                    params.Add(paramName, vals(i).Trim)
                    paramList.Add(paramName)
                Next i
                param = "(" + Strings.Join(paramList.ToArray, ",") + ")"
            Else
                comOperator = " = "
                params.Add(param, val)
            End If
            injection.Add("[" + rgx.Replace(whereClause, "") + "]" + comOperator + param)

        Next whereClause

        If injection.Count > 0 Then sqlText.Replace("--INJECTION", " WHERE " + String.Join(" AND ", injection.ToArray))

        'Execute the SQL Command
        Dim sqlCmd As New SqlClient.SqlCommand(sqlText.ToString, sqlCon)
        sqlCmd.Parameters.Add("@Count", SqlDbType.Int, 5).Value = count

        'For Each WhereClause As String In queryString.AllKeys
        '    Dim param As String = Strings.Replace(WhereClause, " ", "_")
        '    If queryString(WhereClause).Contains("|") Then
        '        sqlCmd.Parameters.Add("@" + param, SqlDbType.Decimal, 30).Value = queryString(WhereClause).Split(New String() {"|"}, StringSplitOptions.None)(1)
        '    Else
        '        sqlCmd.Parameters.Add("@" + param, SqlDbType.VarChar, 30).Value = queryString(WhereClause)
        '    End If
        '    Debug.Print(sqlCmd.Parameters("@" + param).Value.ToString)
        'Next WhereClause

        For Each param As KeyValuePair(Of String, String) In params
            If Double.TryParse(param.Value, New Double) Then
                sqlCmd.Parameters.Add(param.Key, SqlDbType.Decimal, 30).Value = param.Value
            Else
                sqlCmd.Parameters.Add(param.Key, SqlDbType.VarChar, 30).Value = param.Value
            End If
            Debug.Print(sqlCmd.Parameters(param.Key).Value.ToString)
        Next param

        Debug.Print(sqlCmd.CommandText.ToString)

        Dim sqlReader As System.Data.SqlClient.SqlDataReader = sqlCmd.ExecuteReader()

        'Get the Headers as returned by the SQL Server
        Dim headerRow As New TableHeaderRow
        For i As Integer = 0 To sqlReader.FieldCount - 1
            Dim c As New TableCell 'New Table Cell
            Dim Header As String = sqlReader.GetName(i).ToString
            c.Controls.Add(New LiteralControl(Header))
            headerRow.Cells.Add(c)
        Next i
        With headerRow
            .Font.Bold = True
            .ID = "headerrow"
        End With
        tTable1.Rows.Add(headerRow)

        Dim n As Integer = 0 'This will be used a counter and is only needed to make alternating colors in the table
        While sqlReader.Read()
            Dim r As New TableRow
            For i = 0 To sqlReader.FieldCount - 1
                Dim c As New TableCell
                c.Controls.Add(New LiteralControl(sqlReader(i).ToString.Replace(" ", "&nbsp;").Replace("-", "-&#65279;")))
                r.Cells.Add(c) 'Add the cell to the row
            Next i

            'If the row count is not evenly divisible by 2, the background is gray, else the background is blue-gray
            If (n Mod 2) > 0 Then r.BackColor = Drawing.Color.FromArgb(210, 210, 210) Else r.BackColor = Drawing.Color.FromArgb(202, 224, 245)
            n += 1 'Advance the row count
            tTable1.Rows.Add(r) 'Add the row to the Table
        End While
        sqlCon.Close()

        Return tTable1
    End Function
    Function ToProperCase(ByVal str As String) As String 'Take a string and make the first letter of each word capitalized
        Dim myTI As System.Globalization.TextInfo

        myTI = New System.Globalization.CultureInfo("en-US", False).TextInfo
        str = str.ToLower
        str = myTI.ToTitleCase(str)

        Return str
    End Function
End Module


