import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';
import {
  Form,
  Link,
  useLoaderData,
  useLocation,
  useNavigate,
} from 'react-router-dom';
import Note from '../components/Note';
import React, { useState, useEffect } from 'react';
import ReactPaginate from 'react-paginate';
import { IoAddOutline } from 'react-icons/io5';

export const loader = async () => {
  try {
    const response = await customRequest.get('/tags');
    return response?.data;
  } catch (error) {
    toast.error(error?.response?.data?.message);
    return [];
  }
};

const fetchNotes = async (queryParams) => {
  const response = await customRequest.get('/notes', { params: queryParams });
  return response.data;
};

const useQuery = () => {
  return new URLSearchParams(useLocation().search);
};

const AllNotes = () => {
  const tags = useLoaderData();
  const query = useQuery();
  const navigate = useNavigate();
  const location = useLocation();

  const initialSearchParams = {
    title: query.get('title') || '',
    tagId: query.get('tagId') || '',
    newest: query.get('newest') === 'true' || query.get('newest') === null,
    ownership: query.get('ownership') || 'all',
    pageNumber: parseInt(query.get('pageNumber'), 10) || 1,
  };

  const [formKey, setFormKey] = useState(0);
  const [searchParams, setSearchParams] = useState(initialSearchParams);
  const [data, setData] = useState({ content: [] });

  useEffect(() => {
    const loadData = async () => {
      const fetchedData = await fetchNotes(searchParams);
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
      newest: true,
      ownership: 'all',
      pageNumber: 1,
    };
    setSearchParams(resetParams);
    setFormKey((prev) => prev + 1);
    navigate(location.pathname, { replace: true });
  };

  const handleDelete = (id) => {
    setData((prevData) => ({
      ...prevData,
      content: prevData.content.filter((note) => note.id !== id),
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
                <label>Order</label>
                <select name='newest' defaultValue={searchParams.newest}>
                  <option value='true'>Newest</option>
                  <option value='false'>Oldest</option>
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
        <p className='centered'>No notes found</p>
      ) : (
        <>
          <section className='all-container overflowed'>
            {data.content.map((noteData) => (
              <Note key={noteData.id} {...noteData} onDelete={handleDelete} />
            ))}
          </section>
          <section className='pagination-section'>
            <ReactPaginate
              onPageChange={paginate}
              pageCount={Math.ceil(data.size / 8)}
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

export default AllNotes;
