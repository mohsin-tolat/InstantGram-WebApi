#### Scaffolding the Context Command

dotnet ef dbcontext scaffold "ConnectionStrings" Microsoft.EntityFrameworkCore.SqlServer --project ../InstantGram.Data/ --context ContextName --context-dir .\ContextFolderPath --output-dir .\DbModelPath

e.g.

dotnet ef dbcontext scaffold "Server=PMCLAP1349\SQLEXPRESS;Database=InstantGram_V001;User Id=sa;Password=India@123;" Microsoft.EntityFrameworkCore.SqlServer  --project ../InstantGram.Data/ --context ApplicationDbContext --context-dir .\DBContexts  --output-dir .\DBModels --force

Generating the OpenCover Output for the test case
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

Generating the HTML Reports.
dotnet reportgenerator "-reports:coverage.opencover.xml" "-targetdir:coveragereport" -reporttypes:Html