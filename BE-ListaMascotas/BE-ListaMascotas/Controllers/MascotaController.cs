﻿using AutoMapper;
using BE_ListaMascotas.Models;
using BE_ListaMascotas.Models.DTO;
using BE_ListaMascotas.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_ListaMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
   
        private readonly IMapper _mapper;
        private readonly IMascotaRepository _mascotaRepository;

        public MascotaController(IMapper mapper, IMascotaRepository mascotaRepository)
        {
            _mapper = mapper;
            _mascotaRepository = mascotaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listaMascotas = await _mascotaRepository.GetListaMascotas();

                var listaMascotasDto = _mapper.Map<IEnumerable<MascotaDTO>>(listaMascotas);

                return Ok(listaMascotasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mascota = await _mascotaRepository.GetMascota(id);

                if (mascota == null)
                {
                    return NotFound();
                }

                var mascotaDto = _mapper.Map<MascotaDTO>(mascota);

                return Ok(mascotaDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var mascota = await _mascotaRepository.GetMascota(id);

                if (mascota == null)
                {
                    return NotFound();
                }

                await _mascotaRepository.DeleteMascota(mascota);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post(MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);

                mascota.FechaCreacion = DateTime.Now;

                mascota = await _mascotaRepository.AddMascota(mascota);

                var mascotaItemDto = _mapper.Map<MascotaDTO>(mascota);

                return CreatedAtAction("Get", new { id = mascota.Id }, mascota);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MascotaDTO mascotaDto)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDto);

                if (id != mascota.Id)
                {
                    return BadRequest();
                }

                var mascotaItem = await _mascotaRepository.GetMascota(id);

                if (mascotaItem == null)
                {
                    return NotFound();
                }

                await _mascotaRepository.UpdateMascota(mascota);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
