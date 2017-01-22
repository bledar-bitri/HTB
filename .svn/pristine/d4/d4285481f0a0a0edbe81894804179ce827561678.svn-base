<%
dim sT
if Request.ServerVariables("REMOTE_ADDR") = Request.ServerVariables("LOCAL_ADDR") then
    sT = Request("SessionVar")
    if trim(sT) <> "" then
      Response.Write Session(sT)
    end if
end if
%>
