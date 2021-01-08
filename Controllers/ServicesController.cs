using BackendServices.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace BackendServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private NpgsqlConnection connection;
        private string sql;
        private NpgsqlCommand cmd;

        [HttpGet("people")]
        public async Task<List<PeopleEntity>> GetPeople()
        {
            var listPeople = await GetListPeople();
            if (listPeople.Count < 0)
            {
                return null;
            }
            return listPeople;
        }
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
        [HttpGet("taskByFilter{idpeople}*{state}*{priority}*{fecInicio}*{fecFinal}")]
        public async Task<List<TaskEntity>> GetTasksFilter(int idpeople, string state, string priority, string fecInicio, string fecFinal)
        {
            var task = await GetTaskFilter(idpeople,state,priority,fecInicio,fecFinal);
            if (task.Count < 0)
            {
                return null;
            }
            return task;
        }
        [HttpPut("updateTask{idtask}*{idpeople}*{description}*{state}*{priority}*{fecInicio}*{fecFinal}*{notes}")]
        public  Task<bool> updateTasks(int idtask,int idpeople,string description,string state, string priority,string fecInicio, string fecFinal,string notes)
        {
            Task<bool> num = updateTask(idtask,idpeople,description,state,priority,fecInicio,fecFinal, notes);
            return num;
        }
        [HttpPost("insertTask{idpeople}*{description}*{state}*{priority}*{fecInicio}*{fecFinal}*{notes}")]
        public Task<bool> insertTasks(int idpeople, string description, string state, string priority, string fecInicio, string fecFinal, string notes)
        {
            Task<bool> num = insertTask(idpeople, description, state, priority, fecInicio, fecFinal, notes);
            return num;
        }
        [HttpDelete("deleteTask{id}")]
        public async Task<bool> deleteTasks(int id)
        {
            bool task = await deleteTask(id);
            return task;
        }

        //DATABASE
        private async Task<List<PeopleEntity>> GetListPeople()
        {
            List<PeopleEntity> listPeople = new List<PeopleEntity>();
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT * from public.sp_getPeople()";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listPeople.Add(new PeopleEntity()
                    {
                        idPeople = Int32.Parse(reader["v_idpeople"].ToString()),
                        namePeople = reader["v_namepeople"].ToString()
                    });
                }
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
            return listPeople;
        }
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
                        fechaInicio = reader["v_fecha_inicio"].ToString().Substring(0, 10),
                        fechaFinal = reader["v_fecha_final"].ToString().Substring(0, 10),
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
        private async Task<List<TaskEntity>> GetTaskFilter(int idpeople, string state, string priority, string fecInicio, string fecFinal)
        {
            List<TaskEntity> listTasks = new List<TaskEntity>();
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT * from public.sp_getTasksByFilter("+idpeople+",'" + state + "','"+priority+"','"+fecInicio+"','"+fecFinal+"')";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listTasks.Add(new TaskEntity()
                    {
                        idTask = Int32.Parse(reader["v_idTask"].ToString()),
                        description = reader["v_description"].ToString(),
                        idPeople = Int32.Parse(reader["v_idpeople"].ToString()),
                        stateTask = reader["v_statetask"].ToString(),
                        priority = reader["v_priority"].ToString(),
                        fechaInicio = reader["v_fecha_inicio"].ToString().Substring(0, 10),
                        fechaFinal = reader["v_fecha_final"].ToString().Substring(0, 10),
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
        private async Task<bool> updateTask(int idtask, int idpeople, string description, string state, string priority, string fecInicio, string fecFinal, string notes)
        {
            bool task = false;
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT public.sp_updateTask("+idtask+",'"+description+"','"+idpeople+"','"+state+"','"+priority+"','"+fecInicio+"','"+fecFinal+"','"+notes+"')";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                task = reader.Read();
                while (reader.Read())
                {
                    task = true;
                }
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
            return task;
        }
        private async Task<bool> insertTask(int idpeople, string description, string state, string priority, string fecInicio, string fecFinal, string notes)
        {
            bool task = false;
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT public.sp_insertTask('" + description + "','" + idpeople + "','" + state + "','" + priority + "','" + fecInicio + "','" + fecFinal + "','" + notes + "')";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                task = reader.Read();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
            return task;
        }
        private async Task<bool> deleteTask(int id)
        {
            bool task = false;
            connection = new NpgsqlConnection("Server= localhost; User Id= postgres; Password= 1234; Port= 5432; Database= backendservices");
            try
            {
                connection.Open();
                sql = @"SELECT * from public.sp_deleteTask(" + id + ")";
                cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                task = reader.Read();
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


