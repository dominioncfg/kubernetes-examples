#Builds the url of the repository  from values.yaml:
{{- define "database-migrator.buildImageFromValues" -}}
 {{ .repositoryUrl }}/{{- .repositoryName }}:{{ .tag }}
{{- end -}}


#Common Labels For Identifying the resources
{{- define "database-migrator.commonLabels" -}}
app: students-service-db-migrator
type: backend
subtype: dot-net-job
version: "{{ .Values.image.tag }}"
{{- end -}}
