#Builds the url of the repository  from values.yaml:
{{- define "backend.buildImageFromValues" -}}
 {{ .repositoryUrl }}/{{- .repositoryName }}:{{ .tag }}
{{- end -}}



#Function for matching labels between: Pods<-Deployment<-Service
{{- define "backend.matchLabels" -}}
service-name: {{ .Release.Name }}-{{ .Chart.Name }}-backend-api
{{- end -}}


#Common Labels For Identifying the resources
{{- define "backend.commonLabels" -}}
app: students-service
type: backend
subtype: dot-net-api
version: "{{ .Values.image.tag }}"
{{- end -}}




#Get Backend Config Secret Name
{{- define "backend.getDatabaseConfigSecretName" -}}
{{- if eq .Values.databaseConnectionStringManagement.createSecret true -}}
 {{ .Release.Name }}-{{ .Chart.Name }}-backend-cfg
{{- else -}}
 {{ .Values.databaseConnectionStringManagement.existingSecretName}}
{{- end -}}
{{- end -}}
