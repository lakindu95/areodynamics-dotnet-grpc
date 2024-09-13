import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '@/components/HomePage.vue';
import FileUpload from '@/components/FileUpload.vue';
import FlightList from '@/components/FlightList.vue';
import EditFlight from '@/components/EditFlight.vue';

const routes = [
  {
    path: '/',
    name: 'HomePage',
    component: HomePage,
    meta: {
      title: 'Aero Dynamics'
    }
  },
  {
    path: '/upload',
    name: 'FileUpload',
    component: FileUpload,
    meta: {
      title: 'Upload Flight Data'
    }
  },
  {
    path: '/flights',
    name: 'FlightList',
    component: FlightList,
    meta: {
      title: 'View Flight List'
    }
  },
  {
    path: '/edit/:flightId',
    name: 'EditFlight',
    component: EditFlight,
    props: true,
    meta: {
      title: 'Edit Flight'
    }
  }
];


const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});
router.beforeEach((to, from, next) => {
  document.title = to.meta.title || 'Aero Dynamics';
  next();
});

export default router;
