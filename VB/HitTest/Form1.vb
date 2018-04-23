Imports Microsoft.VisualBasic
#Region "#usings"
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports System
Imports System.Drawing
Imports System.Windows.Forms
#End Region ' #usings

Namespace HitTest
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			schedulerControl1.Start = New System.DateTime(2015, 9, 2)
			schedulerControl1.DayView.GotoTimeInterval(New TimeInterval(schedulerControl1.Start.AddHours(17), New TimeSpan(2,0,0)))
			LoadData()
		End Sub
		#Region "#hittest"
		Private Sub schedulerControl1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles schedulerControl1.MouseMove
			Dim pos As New Point(e.X, e.Y)
			Dim viewInfo As SchedulerViewInfoBase = schedulerControl1.ActiveView.ViewInfo
			Dim hitInfo As SchedulerHitInfo = viewInfo.CalcHitInfo(pos, False)

			If hitInfo.HitTest = SchedulerHitTest.AppointmentContent Then
				Dim apt As Appointment = (CType(hitInfo.ViewInfo, AppointmentViewInfo)).Appointment
				Text = apt.Subject
			ElseIf schedulerControl1.ActiveView.Type = SchedulerViewType.Day AndAlso hitInfo.HitTest = SchedulerHitTest.Cell Then
				Dim diff As Integer = pos.Y - (CType(hitInfo.ViewInfo, SelectableIntervalViewInfo)).Bounds.Y
				Dim ticksPerPixel As Long = CLng((CType(hitInfo.ViewInfo, SelectableIntervalViewInfo)).Interval.Duration.Ticks / (CType(hitInfo.ViewInfo, SelectableIntervalViewInfo)).Bounds.Height)
				Dim ticksCount As Long = CLng(Fix(ticksPerPixel)) * diff
				Dim actualTime As DateTime = (CType(hitInfo.ViewInfo, SelectableIntervalViewInfo)).Interval.Start.AddTicks(ticksCount)
				Text = actualTime.ToString()
			ElseIf hitInfo.HitTest = SchedulerHitTest.None Then
				Text = Application.ProductName
			Else
				Text = ""
			End If
		End Sub

		Private Sub schedulerControl1_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles schedulerControl1.DragOver
			Dim sc As SchedulerControl = CType(sender, SchedulerControl)
			Dim p As Point = sc.PointToClient(New Point(e.X, e.Y))
			Dim info As SchedulerHitInfo = sc.DayView.ViewInfo.CalcHitInfo(p, True)
			If info.HitTest = SchedulerHitTest.AllDayArea Then
				e.Effect = DragDropEffects.None
			End If
		End Sub
		#End Region ' #hittest

		Private Sub LoadData()
			Dim apt As Appointment = schedulerStorage1.CreateAppointment(AppointmentType.Normal)
			apt.Subject = "Presentation"
			apt.Start = New DateTime(2015, 09, 02, 18,0,0)
			apt.End = apt.Start.AddHours(3)
			apt.Location = "Globus BookStore"
			apt.Description = "The Sassanid Empire"
			apt.LabelId = 3
			apt.HasReminder = True
			apt.Reminder.TimeBeforeStart = New TimeSpan(0, 15, 0)
			schedulerStorage1.Appointments.Add(apt)

		End Sub



	End Class
End Namespace