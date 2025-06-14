﻿using Domain.IntegrationsCore.Entities.Enums;

namespace Application.IntegrationsCore.Interfaces
{
    public interface ILoggerService
    {
        /// <summary>
        /// Create a new log
        /// </summary>
        /// <param name="job"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddLog(EnumJob job);

        /// <summary>
        /// Add an message occurred in one repository stage with exception details and command sql tried to execute
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="error"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="commandSQL"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddMessage(
            EnumStages stage,
            EnumError error,
            EnumMessageLevel logLevel,
            string message,
            string exceptionMessage,
            string commandSQL
        );

        /// <summary>
        /// Add an message occurred in one stage with exception details
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="error"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddMessage(
            EnumStages stage,
            EnumError error,
            EnumMessageLevel logLevel,
            string message,
            string exceptionMessage
        );

        /// <summary>
        /// Add an message occurred in one stage without exception details
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="error"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddMessage(
            EnumStages stage,
            EnumError error,
            EnumMessageLevel logLevel,
            string message
        );

        /// <summary>
        /// Build an error exception message with exception details occurred outside a stage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddMessage(string message, string exceptionMessage);

        /// <summary>
        /// Add an information message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddMessage(string message);

        /// <summary>
        /// Add to list a new record obtained from API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xml"></param>
        /// <returns>ILoggerService</returns>
        public ILoggerService AddRecord(string key, string xml);

        /// <summary>
        /// Set the end execution time
        /// </summary>
        /// <returns>ILoggerService</returns>
        public ILoggerService SetLogEndDate();

        /// <summary>
        /// Persists generated logs in the database
        /// </summary>
        /// <returns></returns>
        public Task CommitAllChanges();

        /// <summary>
        /// Clear records list and messages list
        /// </summary>
        /// <returns>ILoggerService</returns>
        public ILoggerService Clear();

        /// <summary>
        /// Return the Parent Execution GUID
        /// </summary>
        /// <returns></returns>
        public Guid? GetExecutionGuid();
    }
}

