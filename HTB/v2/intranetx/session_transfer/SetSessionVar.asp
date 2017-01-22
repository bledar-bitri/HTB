<%
Session(Request("name")) = Request("value") 
%>
<%=Request("name")  %>  was set to <%= Request("value")  %> 


