import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';
import {
  Form,
  Link,
  useLoaderData,
  useLocation,
  useNavigate,
} from 'react-router-dom';
import ToDo from '../components/ToDo';
import React, { useState, useEffect } from 'react';
import ReactPaginate from 'react-paginate';
import { IoAddOutline } from 'react-icons/io5';

const fetchToDos = async (queryParams) => {
  const response = await customRequest.get('/todos', { params: queryParams });
  return response.data;
};

const useQuery = () => {
  return new URLSearchParams(useLocation().search);
};

const AllToDos = () => {
  const tags = useLoaderData();
  const query = useQuery();
  const navigate = useNavigate();
  const location = useLocation();

  const initialSearchParams = {
    title: query.get('title') || '',
    tagId: query.get('tagId') || '',
    hideDone: query.get('hideDone') === 'true',
    ownership: query.get('ownership') || 'all',
    pageNumber: parseInt(query.get('pageNumber'), 10) || 1,
  };

  const [formKey, setFormKey] = useState(0);
  const [searchParams, setSearchParams] = useState(initialSearchParams);
  const [data, setData] = useState({ content: [] });

  useEffect(() => {
    const loadData = async () => {
      const fetchedData = await fetchToDos(searchParams);
      setData(fetchedData);
    };

    loadData();
  }, [searchParams]);

  const updateQueryParams = (params) => {
    const queryParams = new URLSearchParams(params);
    navigate(`?${queryParams.toString()}`, { replace: true });
  };

  const handleSearch = (event) => {
    event.preventDefault();
    const formData = new FormData(event.target);
    const newSearchParams = {};
    formData.forEach((value, key) => {
      newSearchParams[key] =
        value === 'true' ? true : value === 'false' ? false : value;
    });
    setSearchParams(newSearchParams);
    updateQueryParams(newSearchParams);
  };

  const paginate = (selectedPage) => {
    const newParams = {
      ...searchParams,
      pageNumber: selectedPage.selected + 1,
    };
    setSearchParams(newParams);
    updateQueryParams(newParams);
  };

  const resetSearch = () => {
    const resetParams = {
      title: '',
      tagId: '',
      hideDone: false,
      ownership: 'all',
      pageNumber: 1,
    };
    setSearchParams(resetParams);
    setFormKey((prev) => prev + 1);
    navigate(location.pathname, { replace: true }); // Clear URL parameters
  };

  const handleDelete = (id) => {
    setData((prevData) => ({
      ...prevData,
      content: prevData.content.filter((todo) => todo.id !== id),
    }));
  };

  return (
    <>
      <div className='add-actions'>
        <Link to={'add'} className='add-new-link'>
          {' '}
          <button>
            <IoAddOutline />
          </button>
        </Link>
      </div>
      <section className='form-section-all'>
        <div className='form-container-filter'>
          <Form onSubmit={handleSearch} key={formKey}>
            <div className='form-filter'>
              <div className='input-group'>
                <label>Title</label>
                <input
                  type='text'
                  name='title'
                  defaultValue={searchParams.title}
                />
              </div>
              <div className='input-group'>
                <label>Tag</label>
                <select name='tagId' defaultValue={searchParams.tagId}>
                  <option value=''>-</option>
                  {tags.map((item) => (
                    <option key={item.id} value={item.id}>
                      {item.title}
                    </option>
                  ))}
                </select>
              </div>
              <div className='input-group'>
                <label>Show</label>
                <select name='ownership' defaultValue={searchParams.ownership}>
                  <option value='all'>All</option>
                  <option value='mine'>Mine</option>
                  <option value='shared'>Shared with me</option>
                </select>
              </div>
              <div className='input-group'>
                <label>Hide Done</label>
                <select name='hideDone' defaultValue={searchParams.hideDone}>
                  <option value='false'>No</option>
                  <option value='true'>Yes</option>
                </select>
              </div>
            </div>
            <div className='filter-actions'>
              <button type='submit'>Search</button>
              <button type='button' onClick={resetSearch}>
                Reset
              </button>
            </div>
          </Form>
        </div>
      </section>
      {data && data.content && data.content.length === 0 ? (
        <p className='centered'>No todos found</p>
      ) : (
        <>
          <section className='all-container overflowed'>
            {data.content.map((todoData) => (
              <ToDo key={todoData.id} {...todoData} onDelete={handleDelete} />
            ))}
          </section>
          <section className='pagination-section'>
            <ReactPaginate
              onPageChange={paginate}
              pageCount={Math.ceil(data.size / 10)}
              previousLabel={'Prev'}
              nextLabel={'Next'}
              containerClassName={'pagination'}
              pageLinkClassName={'page-number'}
              previousLinkClassName={'page-number'}
              nextLinkClassName={'page-number'}
              activeLinkClassName={'active'}
            />
          </section>
        </>
      )}
    </>
  );
};

export default AllToDos;
