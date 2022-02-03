import Vue from 'vue';
import Vuetify from 'vuetify/lib/framework';

Vue.use(Vuetify);

import de from 'vuetify/lib/locale/de';

export default new Vuetify({
  lang: {
    locales: { de },
    current: 'de',
  },
});
