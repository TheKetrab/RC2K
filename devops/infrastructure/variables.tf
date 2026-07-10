variable "cosmos_api_key" {
  description = "CosmosDB primary master key"
  type        = string
  sensitive   = true
}

variable "security_salt" {
  description = "bcrypt salt used for password hashing"
  type        = string
  sensitive   = true
}

variable "mailing_sftp_app_password" {
  description = "Google App Password for the mailing sender account"
  type        = string
  sensitive   = true
}

variable "captcha_secret_key" {
  description = "reCAPTCHA v2/v3 server-side secret key"
  type        = string
  sensitive   = true
}

variable "application_insights_connection_string" {
  description = "Application Insights connection string (includes instrumentation key)"
  type        = string
  sensitive   = true
}

variable "blobstorage_connectionstring" {
  description = "Azure Blob Storage connection string"
  type        = string
  sensitive   = true
}

variable "keda_client_id" {
  description = "Client id of KEDA app querying Application Insights"
  type        = string
  sensitive   = true
}

variable "keda_client_password" {
  description = "Client password of KEDA app querying Application Insights"
  type        = string
  sensitive   = true
}
