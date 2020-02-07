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
        sh 'dotnet publish --self-contained -r win10-x64 -o App-Publish'
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