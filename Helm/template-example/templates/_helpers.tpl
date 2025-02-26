#Named Templates is a good practice to use the <ChartName>.<TemplateName> to avoid collisions
{{- define "template-example.labels" -}}
app: {{ .Chart.Name }}
release: {{ .Release.Name }}
{{- end -}}



#Validation Function to validate a port. Another way to do validation
{{- define "template-example.validator.validatePort" -}}
{{- $port := int  . -}}
{{- if and (ge $port 100) (le $port 200) -}}
  {{- /* Port is valid */ -}}
{{- else -}}
  {{- fail (printf "Port %d must be between 100 and 200" $port) -}}
{{- end -}}
{{- end -}}