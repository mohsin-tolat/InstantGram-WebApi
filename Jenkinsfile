pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Build Started'
        sh(script:  'dotnet build', returnStdout: true)
      }
    }

    stage('Unit Testing') {
      steps {
        sh(script:  'dotnet test', returnStdout: true)
      }
    }

    stage('Publish') {
      steps {
        echo 'Publish Started'
        bat 'cd InstantGram.Api'
        echo 'Publish project'
        sh(script: 'dotnet publish InstantGram.sln -c release -r win10-x64 --self-contained -o App-Publish', returnStdout: true)
      }
    }

    stage('Migration') {
      steps {
        echo 'Migration Started'
        echo 'Here, all the Artifacts will be moved to server.'
      }
    }

    stage('End') {
      steps {
        echo 'Deployment Done'
      }
    }

  }
}