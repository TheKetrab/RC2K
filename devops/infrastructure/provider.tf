provider "azurerm" {
  features {
  }
  use_cli                         = true
  use_oidc                        = false
  resource_provider_registrations = "none"
  subscription_id                 = "3e4efcf0-a92c-4e12-9b67-4cef3d920ecb"
  environment                     = "public"
  use_msi                         = false
}
