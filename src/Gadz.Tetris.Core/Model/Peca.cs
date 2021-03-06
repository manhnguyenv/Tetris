﻿using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public class Peca {

        static IDictionary<TiposDePeca, CoresDePeca> _cores = new Dictionary<TiposDePeca, CoresDePeca> {
                { TiposDePeca.I, CoresDePeca.Ciano },
                { TiposDePeca.T, CoresDePeca.Roxo},
                { TiposDePeca.L, CoresDePeca.Laranja},
                { TiposDePeca.J, CoresDePeca.Azul},
                { TiposDePeca.O, CoresDePeca.Amarelo},
                { TiposDePeca.S, CoresDePeca.Verde},
                { TiposDePeca.Z, CoresDePeca.Vermelho}
        };

        #region properties

        public int Rotacao { get; private set; }
        public CoresDePeca Cor => PegarCorPara(Tipo);
        public Forma Forma => FormaFactory.Desenhar(Tipo, Posicao, Rotacao);
        public Ponto Posicao { get; private set; }
        public TiposDePeca Tipo { get; private set; }
        public Tabuleiro Tabuleiro { get; private set; }
        public Bloco[] Blocos => Forma.Blocos;

        #endregion

        #region constructors

        internal Peca(TiposDePeca tipo, Ponto posicao, int rotacao, Tabuleiro tabuleiro) {
            Tipo = tipo;
            Posicao = posicao;
            Tabuleiro = tabuleiro;
            Rotacao = rotacao;
        }

        private Peca(Peca clone) {
            Rotacao = clone.Rotacao;
            Posicao = clone.Posicao;
            Tipo = clone.Tipo;
            Tabuleiro = clone.Tabuleiro;
        }

        #endregion

        #region methods

        public void Girar() {

            Rotacao = Rotacao == 3 ? 0 : ++Rotacao;

            int maxX = Posicao.X;
            int minX = Posicao.X;

            foreach(var b in Blocos) {
                if(b.X > maxX) {
                    maxX = b.X;
                }
            }

            foreach (var b in Blocos) {
                if (b.X < minX) {
                    minX = b.X;
                }
            }

            if(maxX > Tabuleiro.Largura-1) {

                int excesso = maxX - (Tabuleiro.Largura - 1);
                for (int i = 0; i < excesso; i++) {
                    MoverEsquerda();
                }
            }

            if(minX < 0) {
                for (int i = minX; i < 0; i++) {
                    MoverDireita();
                }
            }
        }

        public void MoverBaixo() {
            Posicao = new Ponto(Posicao.X, Posicao.Y + 1);
        }

        public void MoverDireita() {
            Posicao = new Ponto(Posicao.X + 1, Posicao.Y);
        }

        public void MoverEsquerda() {
            Posicao = new Ponto(Posicao.X - 1, Posicao.Y);
        }

        public Peca Clonar() {
            return new Peca(this);
        }
                
        public static CoresDePeca PegarCorPara(TiposDePeca index) {
            try {
                return _cores[index];
            } catch (KeyNotFoundException) {
                return CoresDePeca.Transparente;
            }
        }

        #endregion

        #region overrided methods

        public override string ToString() {
            return $"{Tipo.ToString()} ({Forma.ToString()})";
        }

        public override bool Equals(object obj) {
            if(obj is Peca p) {
                return ToString().Equals(p.ToString());
            }
            return false;
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        #endregion
    }
}
