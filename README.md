# ChurnAnalyzer
Work in progress...
- Commit statistics
- File coupling
- Churned files

#Usage
git log --all --numstat --pretty=format:'--%an%n%ci' --reverse > git.log
Create gitLogs folder
Place git.log into gitLogs folder
dotnet run