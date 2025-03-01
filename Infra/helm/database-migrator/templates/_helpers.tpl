#Builds the url of the repository  from values.yaml:
{{- define "database-migrator.buildImageFromValues" -}}
 {{ .repositoryUrl }}/{{- .repositoryName }}:{{ .tag }}
{{- end -}}


#Get Backend Config Secret Name
{{- define "database-migrator.getDatabaseConfigSecretName" -}}
{{- if eq .Values.databaseConnectionStringManagement.createSecret true -}}
 {{ .Release.Name }}-{{ .Chart.Name }}-migrator-cfg
{{- else -}}
 {{ .Values.databaseConnectionStringManagement.existingSecretName}}
{{- end -}}
{{- end -}}


#Common Labels For Identifying the resources
{{- define "database-migrator.commonLabels" -}}
app: students-service
type: backend
subtype: dot-net-job
{{- end -}}
