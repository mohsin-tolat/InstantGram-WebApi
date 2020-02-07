pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Build Started'
        sh 'dotnet build'
      }
    }

    stage('Unit Testing') {
      steps {
        sh 'dotnet test'
      }
    }

    stage('Publish') {
      steps {
        echo 'Publish Started'
        sh 'dotnet publish -c release -r win10-x64 --self-contained true /p:useapphost=true'
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