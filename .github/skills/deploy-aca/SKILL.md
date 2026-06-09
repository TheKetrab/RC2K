---
name: deploy-aca
description: Deployment for RC2K Hub Dynamic App to Azure Container Apps.
---

Note: if any step fails, than stop operation and notify user!

To deploy new version for RC2K Hub, you should:

1. Ask user first what will be the new version - it should be eg. 1.3.5

2. Change version prefix in Directory.Builds.props: <VersionPrefix>1.3.5</VersionPrefix>

3. Create Cosmos Db backup
- verify if there is C:\Projekty\rc2k\utils\cosmos-backup directory
- create new directory with date in utils/cosmos-backup
- prepare migrationsettings.json with proper directory sink (new directory you've just created)
- put it in directory dmt-3.0.0-win-x64 and run dmt.exe there
- verify it succeeded and created proper jsons

3. Create new version
- merge feature branch into main branch
- create annotated tag: $ git tag -a v1.3.5 -m "Version 1.3 (fix 5)" (NOTE: if minor is zero, then do not add "(fix 0)")
- push tag: $ git push origin --tags

3. Deploy new container version
- run docker desktop to run docker deamon
- see C:\Projekty\rc2k\blazor\RC2K\misc\deploy.md
- build docker image
- push docker image
- create new revision: Azure Portal > Container App > "Application/Revisions and replicas"
- restart revision so that it fetches new image

4. Deploy new static version:
- run gh workflow: https://github.com/TheKetrab/RC2K/actions/workflows/deploy-static-app.yml

