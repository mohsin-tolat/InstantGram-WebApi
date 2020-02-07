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
      }
    }

    stage('Migration') {
      steps {
        echo 'Migration Started'
      }
    }

    stage('Deploy') {
      steps {
        echo 'Deployment Started'
      }
    }

    stage('End') {
      steps {
        echo 'Deployment Done'
      }
    }

  }
}