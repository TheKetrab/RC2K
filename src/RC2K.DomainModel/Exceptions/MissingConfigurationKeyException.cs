namespace RC2K.DomainModel.Exceptions;

public class MissingConfigurationKeyException(string key) : Exception($"{key} configuration is missing");