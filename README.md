## Requirements
- Unity 2022.3.2xx
- Varjo headset
- Dx12 compatible Nvidia RTX GPU
- Windows 10/ Windows 11

## Commit contribution rules
- It is not recommended to push directly to ```main```-branch as that can cause issues down the line when multiple people try to push their changes at the same time. 
In some cases like a quick bug fix, it is fine.
- Use branches for avoiding conflicts and loss of changes. It's much easier to merge stuff into ```main```-branch when you don't have to worry about losing changes.
  -  Create a branch in which you always work on. Make sure that the branch is up to date with ```main```-branch.
  -  Create a branch for different tasks like ```hand-tracking```, ```debugger-overlay```etc.
- Once you are done working on a feature or a bug fix:
  - Switch to ```main```-branch
  - Make sure that it is up to date with ```git pull```.  **Note**, sometimes you need to run ```git pull --rebase```, if that doesn't work then you need to merge manually.
  - git rebase ```branch-name```. This will bring commit(s) from your branch to ```main```-branch and try to put as the newest commit(s) in the history (if ```main```and your branch are in sync).
  - Once rebase has been succeful you can ```git push```. If not fix merge conflicts.

## Tools for merging conflicts.
Look up online but I personally have used Vscode for fixing conflicts but there are other tools available.
![image](https://github.com/warriormaster12/VarjoDemo/assets/33091666/978839bf-e0fe-4210-b46a-2aa953ae4d5d)


