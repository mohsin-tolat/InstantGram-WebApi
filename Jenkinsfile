pipeline {
    agent any
    // parameters {
    //       string(name: 'PERSON', defaultValue: 'Mr Jenkins', description: 'Who should I say hello to?')
    //       text(name: 'BIOGRAPHY', defaultValue: '', description: 'Enter some information about the person')
    //       booleanParam(name: 'TOGGLE', defaultValue: true, description: 'Toggle this value')
    //       choice(name: 'CHOICE', choices: ['One', 'Two', 'Three'], description: 'Pick something')
    //       password(name: 'PASSWORD', defaultValue: 'SECRET', description: 'Enter a password')
    //   } 
    parameters {
        choice(name: 'BuildType', choices: ['Development', 'QA', 'UAT', 'Production'], description: 'Select Build Type to generate package for configurations.')
    }
    stages {
        stage('Build') {
            steps {
                echo "Build Started"
                echo "Built Type: ${params.BuildType}"
                sh(script: 'dotnet build', returnStdout: true, returnStatus: true)
            }
        }
        stage('Unit Testing') {
            steps {
                sh(script: 'dotnet test', returnStdout: true, returnStatus: true)
            }
        }
        stage('Generate Package') {
            steps {
                echo "Publish Started"
                sh(script: 'dotnet publish InstantGram.sln -c release -r win10-x64 --self-contained -o App-Publish', returnStdout: true, returnStatus: true)
            }
        }
        stage('Migration') {
            steps {
                archiveArtifacts artifacts: 'App-Publish/*.*', onlyIfSuccessful: true
            }
        }
        stage('Deployment') {
            steps {
                echo "Deployment Started"
                echo "All The Copy Paste and Start/Stop Service will be done."
            }
        }
    }
}
