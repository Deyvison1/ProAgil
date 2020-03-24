import { RedeSocial } from './RedeSocial';
import { Evento } from './Evento';
export interface Palestrante {
        nome: string;
        miniCurriculo: string;
        id: number;
        imagemURL: string;
        telefone: string;
        email: string;
        redesSociais: RedeSocial[];
        palestrantesEventos: Evento[];
}
