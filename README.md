# Introduction 
As requested in the task description I build a small API service that implements simple sale functionality as well as some statistic/reporting.

# Getting Started
With the sales controller you can post a sales event to the service. It will be saved to a database that needs to be configured within the appsettings.
As well there is a reporting controller with which you can request the statistics and reports of the posted sales.

# Build and Deployment

For building and running the solution you can also use the provided azure pipeline.  It uses a docker file to build the app and deploys it then to a newly created webservice. If you want to use the pipeline, simply set the variables in the pipeline file. (The DB connection string can’t be set there, but the pipeline will create the setting and you can then link this setting to a keyVault)
The database will not be created. Please just provide a connection string to an Azure DB.
I also deployed the app to my private azure cloud. You can find the link to it in the mail.

# Bonus and Tests

As a bonus I added a database layer, a Dockerfile (together with a pipeline) and hosted the app as mentioned.
As another small bonus I also added two tracking service (EventHub and Api call to an external service). These two can be configured in the appsettings (and the pipelinefile). External service tracking is the preferred implementation if both are activated.
I also added unit-, integration- and load-tests. 
