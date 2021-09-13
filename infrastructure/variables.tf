variable "resourceGroupName" {
  type        = string
  description = "Name of target portal resource group"
}

variable "location" {
  type        = string
  description = "Resources location"
}

variable "prefix" {
  type        = string
  description = "Prefix used by created resources"
}