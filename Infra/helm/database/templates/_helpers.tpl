#Builds the url of the repository  from values.yaml:
#Ex:
#host.minikube.internal:5000/frontend-server:v1.4
{{- define "database.buildImageFromValues" -}}
 {{ .repositoryUrl }}/{{- .repositoryName }}:{{ .tag }}
{{- end -}}



#Common Labels For Identifying the resources
{{- define "database.commonLabels" -}}
app: students-db
type: database
subtype: pg-sql
version: "{{ .Values.image.tag }}"
{{- end -}}

#Function for matching labels between: Pods<-StatefulSet<-Service
{{- define "database.matchLabels" -}}
service-name: {{ .Release.Name }}-{{ .Chart.Name }}-database-server
{{- end -}}
