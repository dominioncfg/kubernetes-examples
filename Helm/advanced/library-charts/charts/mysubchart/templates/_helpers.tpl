#Named Templates is a good practice to use the <ChartName>.<TemplateName> to avoid collisions
{{- define "library-chat.labels" -}}
app: {{ .Chart.Name }}
release: {{ .Release.Name }}
{{- end -}}
