﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        int SaveChanges();

        Task ExecuteResilientTransacion(Func<Task> action);

        Task<T> ExecuteResilientTransacion<T>(Func<Task<T>> action);
    }
}
