﻿using Gadz.Tetris.Data;
using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;

namespace Gadz.Tetris {

    public class GameController {

        #region fields

        Tabuleiro _tabuleiro;

        #endregion

        #region eventos

        public event GameActionEventHandler OnRefresh;
        public event GameActionEventHandler OnEnd;
        public event GameActionEventHandler OnClear;
        public event GameActionEventHandler OnMove;
        public event GameActionEventHandler OnSlide;

        #endregion

        #region properties

        public bool Playing => _tabuleiro.EstaJogando;
        public int BoardWidth => _tabuleiro.Largura;
        public int BoardHeight => _tabuleiro.Altura;
        public int Speed => _tabuleiro.Velocidade;
        public int Lines => _tabuleiro.Linhas;
        public Bloco[,] Matrix => _tabuleiro.Matriz;
        public TimeSpan Time => _tabuleiro.Duracao;
        public int Score => _tabuleiro.Pontos;
        public int Level => _tabuleiro.Nivel;
        public ITabuleiroEstado State => _tabuleiro.Estado;
        public Ponto CurrentPiecePosition => _tabuleiro.PecaAtual.Posicao;
        public Tabuleiro CurrentBoard => _tabuleiro;

        #endregion

        #region constructors

        private GameController(int largura, int altura) {

            if(largura < 10) {
                throw new ArgumentException(nameof(largura));
            }

            if(altura < 10) {
                throw new ArgumentException(nameof(altura));
            }

            _tabuleiro = new Tabuleiro(new EstatisticasRepository(), largura, altura);
            _tabuleiro.QuandoAtualizar += ()=> { OnRefresh?.Invoke(); };
            _tabuleiro.QuandoTerminar += ()=> { OnEnd?.Invoke();  };
            _tabuleiro.QuandoLimpar += ()=> { OnClear?.Invoke(); };
            _tabuleiro.QuandoMover += ()=> { OnMove?.Invoke(); };
            _tabuleiro.QuandoDeslizar += ()=> { OnSlide?.Invoke(); };
        }

        #endregion

        public static GameController Create(int largura, int altura) {
            return new GameController(largura, altura);
        }

        public void SmashDown() {
            _tabuleiro.Cair();
        }

        public void RunRight()=> _tabuleiro.CorrerDireita();

        public void RunLeft()=> _tabuleiro.CorrerEsquerda();

        public void Start() => _tabuleiro.Iniciar();

        public void MoveDown() => _tabuleiro.MoverBaixo();

        public void MoveRight()=> _tabuleiro.MoverDireita();

        public void MoveLeft()=> _tabuleiro.MoverEsquerda();

        public void Pause()=> _tabuleiro.Pausar();

        public void Save()=> _tabuleiro.Salvar();

        public void Exit() {
            _tabuleiro.Terminar();
            Save();
        }

        public void Rotate()=> _tabuleiro.Rotacionar();

        public void Continue()=> _tabuleiro.Continuar();

        public void Restart() => _tabuleiro.Reiniciar();

        public IEnumerable<Bloco> GetActualBlocks()=> _tabuleiro.PecaAtual.Blocos;

        public IEnumerable<Bloco> GetNextBlocks()=> _tabuleiro.ProximaPeca.Blocos;

        public int GetMaxScore() {
            return _tabuleiro.RecordeMaximo;
        }
    }
}
