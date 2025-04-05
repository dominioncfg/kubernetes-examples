
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
app: students-bff
type: backend
subtype: dot-net-api
version: "{{ .Values.image.tag }}"
{{- end -}}
