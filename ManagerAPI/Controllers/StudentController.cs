﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerAPI.Dtos;
using ManagerAPI.Entities;
using ManagerAPI.Common;
using ManagerAPI.Repositories;

namespace ManagerAPI.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentController : ControllerBase
    {

        private readonly StudentRepository studentsRepository;

        public StudentController(IRepository<Student> studentsRepository)
        {
            this.studentsRepository = (StudentRepository)studentsRepository;
        }

        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAsync()
        {

            var students = (await studentsRepository.GetAllSync())
                        .Select(student => student.AsDto());            
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetByIdAsync(Guid id)
        {
            var item = await studentsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CreateStudentDto createStudentDto)
        {
            var student = new Student
            {
                firstName = createStudentDto.firstName,
                lastName = createStudentDto.lastName,
                email = createStudentDto.email,
                career = createStudentDto.career,
                school = createStudentDto.school,
                signedUp = createStudentDto.signedUp,
            };

            await studentsRepository.CreateAsync(student);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = student.StudentId }, student);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateStudentDto updateStudentDto)
        {
            var existingItem = await studentsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.firstName= updateStudentDto.firstName;
            existingItem.lastName= updateStudentDto.lastName;
            existingItem.email= updateStudentDto.email;

            await studentsRepository.UpdateAsync(existingItem);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await studentsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await studentsRepository.RemoveAsync(item.StudentId);

            return NoContent();
        }

    }
}
