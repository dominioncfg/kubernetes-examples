#Builds the url of the repository  from values.yaml:
#Ex:
#host.minikube.internal:5000/frontend-server:v1.4
{{- define "frontend.buildImageFromValues" -}}
 {{ .repositoryUrl }}/{{- .repositoryName }}:{{ .tag }}
{{- end -}}



#Function for matching labels between: Pods<-Deployment<-Service
{{- define "frontend.matchLabels" -}}
service-name: {{ .Release.Name }}-{{ .Chart.Name }}-frontend-server
{{- end -}}


#Common Labels For Identifying the resources
{{- define "frontend.commonLabels" -}}
app: students-service
type: frontend
subtype: http-server
{{- end -}}



#Get Backend Config Secret Name
{{- define "frontend.getConfigSecretName" -}}
{{- if eq .Values.backendConfig.useNewSecret true -}}
 {{ .Release.Name }}-{{ .Chart.Name }}-backend-cfg
{{- else -}}
 {{ .Values.backendConfig.existingSecretName}}
{{- end -}}
{{- end -}}