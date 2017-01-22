<%
Class tNG_LinkedTrans
	Public masterTNG
	Public detailTNG
	Public linkField

	Private Sub Class_Initialize()
		Set this = Me
	End Sub
	
	Private Sub Class_Terminate()
	End Sub

	Public Sub Init (ByRef masterTNG__param, ByRef detailTNG__param)
		Set masterTNG = masterTNG__param
		Set detailTNG = detailTNG__param
	End Sub

	'===========================================
	' Inheritance
	'===========================================
	Public this
	
	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
	End Sub
	'===========================================
	' End Inheritance
	'===========================================
	
	Public Sub setLink (linkField__param)
		linkField = linkField__param
	End Sub


	Public Function Execute()
		If KT_isSet(masterTNG.getError()) Then
			Set Execute = this.onError()
		Else
			Set Execute = this.onSuccess()
		End If
	End Function
	
	Public Function onSuccess()
		detailTNG.setColumnValue linkField, masterTNG.getPrimaryKeyValue()
		detailTNG.executeSubSets = false
		detailTNG.setStarted true
		detailTNG.compileColumnsValues
		detailTNG.doTransaction
		
		Set onSuccess = detailTNG.getError()
	End Function

	Public Function onError()
		If detailTNG.isStarted() Then
			' if 2nd transaction has started
			If Not KT_isSet(detailTNG.getError()) Then
				' if it did not throw any error
				detailTNG.rollBackTransaction (masterTNG.getError())
			End If
		Else
			detailTNG.setColumnValue linkField, masterTNG.getPrimaryKeyValue()
			detailTNG.executeSubSets = false
			detailTNG.setError masterTNG.getError()
			detailTNG.setStarted true
			detailTNG.compileColumnsValues
			detailTNG.doTransaction			
		End If
		Set onError = nothing
	End Function
End Class	
	
%>