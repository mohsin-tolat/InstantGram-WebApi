import groovy.json.JsonSlurper
def cfg = ""


def storeNewValue(serverAdress) {
    def json = new JsonSlurper()
    echo "WORKSPACE_PATH: ${env.WORKSPACE}";
    appSettings = json.parse(new File("${env.WORKSPACE}/App-Publish/appsettings.json"))
    appSettings['ConnectionStrings'].InstantGramDbContext = serverAdress
    cfg = appSettings
    //return appSettings['ConnectionStrings'].ServerGateway
} 

pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Build Started'
        sh(script:  'dotnet build', returnStdout: true, returnStatus: true)
      }
    }

    stage('Unit Testing') {
      steps {
        sh(script:  'dotnet test', returnStdout: true, returnStatus: true)
      }
    }

    stage('Publish') {
      steps {
        echo 'Publish Started'
        sh(script: 'dotnet publish InstantGram.sln -c release -r win10-x64 --self-contained -o App-Publish', returnStdout: true, returnStatus: true)
        echo 'Update Connection Strings'
        storeNewValue("Server=TESTSERVER\\SQLEXPRESS;Database=InstantGram_V001;User Id=sa;Password=India@TEST;")
      }
    }

    stage('Migration') {
      steps {
        echo 'Migration Started'
        echo 'Here, all the Artifacts will be moved to server.'
      }
    }
  }
}
