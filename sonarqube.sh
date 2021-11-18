dotnet sonarscanner begin //k:"Monitor" //d:sonar.cs.nunit.reportsPaths="%CD%\NUnitResults.xml"
if [ $? -eq 0 ];then
   dotnet build ESaniagro.Monitor.sln
fi

if [ $? -eq 0 ];then
   dotnet sonarscanner end
fi