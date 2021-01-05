using BackendServices.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BackendServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private NpgsqlConnection connection;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;

        

        [HttpGet("Tasks")]
        public async Task<List<TaskEntity>> GetTasks()
        {
            
            var listTasks = await GetListTasks();
            if (listTasks.Count < 0)
            {
                return null;
            }
            return listTasks;
        }
        [HttpGet("taskById{id}")]
        public async Task<TaskEntity> GetTaskId(int id)
        {
            var task = await GetTask(id);
            if (task==null)
            {
                return task;
            }
            return task;
        }


        //DATABASE
        private async Task<List<TaskEntity>> GetListTasks()
        {
            List<TaskEntity> listTasks = new List<TaskEntity>();
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT * from public.sp_getTasks()";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                TaskEntity task;
                while (reader.Read())
                {
                    listTasks.Add(new TaskEntity()
                    {
                        idTask = Int32.Parse(reader["v_idTask"].ToString()),
                        description = reader["v_description"].ToString(),
                        idPeople = Int32.Parse(reader["v_idpeople"].ToString()),
                        stateTask = reader["v_statetask"].ToString(),
                        priority = reader["v_priority"].ToString(),
                        fechaInicio = reader["v_fecha_inicio"].ToString(),
                        fechaFinal = reader["v_fecha_final"].ToString(),
                        notes = reader["v_notes"].ToString()
                    });
                }
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
            return listTasks;
        }
        private async Task<TaskEntity> GetTask(int id)
        {
            TaskEntity task = null;
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT * from public.sp_getTasksByID("+id+")";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    task=new TaskEntity()
                    {
                        idTask = Int32.Parse(reader["v_idTask"].ToString()),
                        description = reader["v_description"].ToString(),
                        idPeople = Int32.Parse(reader["v_idpeople"].ToString()),
                        stateTask = reader["v_statetask"].ToString(),
                        priority = reader["v_priority"].ToString(),
                        fechaInicio = reader["v_fecha_inicio"].ToString(),
                        fechaFinal = reader["v_fecha_final"].ToString(),
                        notes = reader["v_notes"].ToString()
                    };
                }
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
            return task;
        }

    }
}


