FROM circleci/python:3.7-stretch-node-browsers
MAINTAINER Ryan Patrick Kyle "ryan@plotly.com"

RUN sudo apt-get update \
 && sudo wget -O - https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o microsoft.asc.gpg \
 && sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
 && sudo wget https://packages.microsoft.com/config/debian/9/prod.list \
 && sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
 && sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
 && sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
 && sudo apt-get install -y apt-transport-https \
 && sudo apt-get update \
 && sudo apt-get install -y dotnet-sdk-5.0
